# Skynetz FaleMais

Simulador de chamadas nacionais para o produto fictício **FaleMais**, da Skynetz. O sistema compara o custo de uma ligação com e sem plano, persiste cada simulação e apresenta ao visitante o histórico associado ao navegador atual.

## Objetivos

- Apresentar o cálculo de forma transparente ao cliente.
- Separar regras de negócio, casos de uso, persistência e interface.
- Manter a aplicação preparada para evoluções sem introduzir complexidade desnecessária.
- Oferecer uma interface própria e responsiva em vez de expor diretamente os controles padrão do Web Forms.

## Tecnologias

- C# 7.3
- ASP.NET Web Forms
- .NET Framework 4.8
- Entity Framework 6.4.4 (Code First Migrations)
- SQL Server / LocalDB
- NUnit para testes automatizados
- Bootstrap 5, CSS próprio e Microsoft AJAX do Web Forms (`ScriptManager` e `UpdatePanel`)

## Funcionalidades

- Seleção de DDD de origem e destino, duração e plano FaleMais.
- Comparação do valor com e sem o plano selecionado.
- Comparativo dos três planos para a mesma rota e duração; essa consulta não grava no histórico.
- Registro das simulações no SQL Server.
- Histórico das 20 simulações mais recentes do visitante atual.
- Identificação anônima por GUID persistido em cookie e associado às simulações.
- Atualização parcial do formulário, resultados, comparativo e histórico pelo `UpdatePanel`.
- Testes automatizados NUnit para regras de Domain e casos de uso de Application, sem dependência do SQL Server.
- Interface responsiva com identidade visual Skynetz.
- Mensagem para rotas sem tarifa cadastrada.

## Regras de negócio

As tarifas são direcionais: a tarifa de `011 → 016` pode ser diferente da tarifa de `016 → 011`.

| Origem | Destino | Tarifa por minuto |
|---|---|---:|
| 011 | 016 | R$ 1,90 |
| 016 | 011 | R$ 2,90 |
| 011 | 017 | R$ 1,70 |
| 017 | 011 | R$ 2,70 |
| 011 | 018 | R$ 0,90 |
| 018 | 011 | R$ 1,90 |

| Plano | Minutos incluídos |
|---|---:|
| FaleMais 30 | 30 |
| FaleMais 60 | 60 |
| FaleMais 120 | 120 |

- **Sem plano:** `minutos × tarifa por minuto`.
- **Com plano:** `máximo(0, minutos − franquia) × tarifa × 1,10`.
- O fator `1,10` representa o acréscimo de 10% sobre os minutos excedentes.
- O custo de aquisição do plano não está incluído.
- Rotas que não constam na tabela não são calculadas.

O cálculo está centralizado em `CalculadoraDeLigacao`, no Domain. A página e os casos de uso não repetem a fórmula, facilitando manutenção e testes sem acoplar o cálculo ao Web Forms ou ao EF6.

## Arquitetura

A solução aplica os princípios de **Clean Architecture** de forma pragmática: regras de negócio independentes no centro e detalhes de interface e persistência nas camadas externas.

```text
Skynetz.slnx
├── Skynetz.Domain/          # Entidades e regras puras do negócio
├── Skynetz.Application/     # Casos de uso, commands/queries, DTOs e contratos
├── Skynetz.Infrastructure/  # EF6, SQL Server, repositórios e migrations
├── Skynetz.Web/             # ASP.NET Web Forms, interface e composição
└── Skynetz.Tests/           # Testes NUnit para Domain e Application
```

Direção das dependências:

```text
Application ──► Domain
Infrastructure ──► Application e Domain
Web ──► Application, Infrastructure e Domain
```

O Web é o ponto de entrada e composição da aplicação (`ApplicationComposition`). Ele instancia os casos de uso com as implementações concretas dos repositórios. As páginas chamam os casos de uso; não acessam `DbContext` diretamente.

### Responsabilidades

- **Domain:** `Tarifa`, `Plano`, `Simulacao` e `CalculadoraDeLigacao`. Não depende de Web Forms, EF6 ou SQL Server.
- **Application:** coordena operações como calcular uma simulação, comparar planos e listar opções/histórico. Define interfaces de repositório em `Abstractions/Persistence`. A comparação é um caso de uso de leitura (`Query`) e não persiste simulações.
- **Infrastructure:** implementa os repositórios com EF6, configura o `DbContext`, migrations e dados iniciais.
- **Web:** recebe os dados da página, resolve o visitante pelo cookie, chama os casos de uso e apresenta os resultados.

## Decisões de arquitetura e produto

### Clean Architecture, sem abstrações desnecessárias

A separação por camadas foi escolhida para permitir evoluir a interface, armazenamento e regras sem misturá-los. Os casos de uso dependem de interfaces de persistência; Infrastructure implementa essas interfaces. Não foram adicionados mediadores, repositório genérico ou dependências extras sem necessidade no escopo atual.

### Cálculo autônomo

`CalculadoraDeLigacao` concentra a regra de preço no Domain. Isso mantém o cálculo desacoplado de controles Web Forms, requisições HTTP e acesso ao banco. A lógica de cálculo não deve ser duplicada na página ou nos repositórios.

### Histórico persistido

Cada simulação válida é armazenada na tabela `Simulacoes`, incluindo um snapshot dos dados usados no cálculo: rota, duração, nome e franquia do plano, tarifa e valores calculados. Assim, alterações futuras nas tarifas não reescrevem o resultado histórico.

### Identificação anônima por navegador

Na primeira visita, o Web Forms gera um GUID e o grava no cookie `Skynetz.VisitanteId`. O identificador é validado a cada requisição e acompanha o comando de cálculo até a entidade `Simulacao` e o banco. O caso de uso do histórico filtra pelo mesmo GUID e retorna somente as simulações daquele visitante.

O cookie é persistente por um ano e usa `HttpOnly`, `SameSite=Lax` e `Secure` quando a requisição está em HTTPS. O GUID é uma chave pseudônima, **não uma identidade autenticada**.

**Limitações conscientes:** o histórico pertence ao navegador/perfil, não a uma pessoa. Limpar o cookie cria um novo histórico visível e não recupera o anterior; outro navegador ou dispositivo não compartilha o histórico. Quem copiar o cookie poderá usar o mesmo identificador. Por isso, o sistema não deve armazenar dados sensíveis com base apenas nesse mecanismo.

### Sem login ou cadastro

Autenticação e criação de usuários foram deixadas fora do escopo atual. O identificador por cookie atende à separação de histórico por navegador com menor complexidade, mas não oferece identidade pessoal ou recuperação entre dispositivos.

### Interface visual própria

A interface usa a identidade visual Skynetz — logotipo tipográfico, símbolo de rotas conectadas, paleta institucional, CSS próprio, hierarquia de informação e comportamento responsivo. Os controles Web Forms continuam sendo usados, mas são apresentados com estilos e componentes adequados ao produto.

### AJAX no Web Forms

O `ScriptManager` está no `Site.Master` e o `UpdatePanel` envolve formulário, resultados, comparação e histórico. O botão ainda executa o evento C# no servidor, mas a região é atualizada de forma assíncrona sem recarregar a página inteira. O sistema não usa um endpoint `fetch` próprio; o recurso AJAX é fornecido pelo Microsoft AJAX do Web Forms.

### Persistência e migrations

A estrutura do banco é versionada com EF6 Code First Migrations. Os dados iniciais de tarifas e planos são inseridos pelo `Seed`. As migrations são parte do código-fonte e devem ser mantidas no Git; o banco local e os arquivos de dados não devem ser commitados.

Consultas de histórico são expressas em LINQ/EF6, usando o GUID como valor tipado no filtro. Não se concatena o valor do cookie em SQL manual.

## Testes automatizados

O projeto `Skynetz.Tests` usa NUnit e referencia somente `Skynetz.Domain` e `Skynetz.Application`. Os testes verificam a fórmula e suas fronteiras, a propagação do GUID e o fluxo de salvamento no caso de uso, o encaminhamento do GUID e o mapeamento do histórico, além da comparação dos planos. Repositórios falsos em memória mantêm esses testes rápidos e independentes do SQL Server; o filtro real do repositório EF6 foi verificado manualmente com navegadores/perfis separados.

No Visual Studio, execute os testes pelo **Test Explorer**. Os cenários de comparação incluem tarifa ausente e validação da regra; não são testes de integração contra um banco real.

## Configuração local

### Pré-requisitos

- Visual Studio com suporte a ASP.NET Web Forms e .NET Framework 4.8.
- SQL Server LocalDB (ou outra instância SQL Server acessível).
- NuGet para restaurar os pacotes registrados em `packages.config`.

### Executar

1. Abra `Skynetz.slnx` no Visual Studio e restaure os pacotes NuGet.
2. Defina `Skynetz.Web` como projeto de inicialização.
3. Confira a connection string `SkynetzConnection` em `Skynetz.Web/Web.config`. Para LocalDB, a configuração de desenvolvimento é semelhante a:

   ```xml
   <add name="SkynetzConnection"
        connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SkynetzDb;Integrated Security=True;MultipleActiveResultSets=True"
        providerName="System.Data.SqlClient" />
   ```

4. No Package Manager Console, aplique as migrations:

   ```powershell
   Update-Database -ProjectName Skynetz.Infrastructure -StartUpProjectName Skynetz.Web
   ```

5. Execute `Skynetz.Web` no Visual Studio. O EF cria/atualiza o banco e o `Seed` registra as tarifas e os planos iniciais.

Para outra instância, altere `Data Source` e, se necessário, autenticação na connection string. Não coloque senhas reais no repositório.

## Cenários de verificação

### Verificação manual

- 011 → 016, 20 minutos, FaleMais 30: R$ 0,00 com plano e R$ 38,00 sem plano.
- 011 → 017, 80 minutos, FaleMais 60: R$ 37,40 com plano e R$ 136,00 sem plano.
- Comparar 011 → 016, 80 minutos: R$ 152,00 sem plano; FaleMais 30 = R$ 104,50, FaleMais 60 = R$ 41,80, FaleMais 120 = R$ 0,00. A comparação não cria itens no histórico.
- 018 → 011, 200 minutos, FaleMais 120: R$ 167,20 com plano e R$ 380,00 sem plano.
- Uma rota sem tarifa apresenta mensagem, sem inventar um preço.
- Dois navegadores/perfis recebem GUIDs distintos e veem históricos distintos.
- O mesmo navegador mantém o histórico enquanto o cookie continuar válido.
- Limpar o cookie gera um novo identificador; registros anteriores permanecem no banco, mas não aparecem no histórico do novo identificador.
- O clique em Calcular atualiza o resultado e o histórico pelo `UpdatePanel`, sem reload completo.

### Testes automatizados

- `CalculadoraDeLigacao`: duração abaixo, igual e acima da franquia; acréscimo de 10%; minutos negativos.
- `CalcularSimulacaoUseCase`: simulação válida salva com o GUID do visitante; rota sem tarifa não salva.
- `ListarHistoricoSimulacoesUseCase`: GUID e quantidade são encaminhados ao repositório; GUID vazio é rejeitado.
- `CompararPlanosUseCase`: os três resultados e diferenças são calculados; rota sem tarifa gera erro de negócio.

