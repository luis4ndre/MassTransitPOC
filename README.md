# MassTransitPOC
MassTransitPOC é um projeto em C# destinado a demonstrar a implementação de Sagas usando o MassTransit, um framework de aplicativos distribuídos para .NET. Sagas são processos de longa duração que gerenciam e coordenam múltiplas transações em sistemas distribuídos, garantindo consistência e confiabilidade.

## Recursos
- Implementa Sagas usando o framework MassTransit
- Demonstra como gerenciar fluxos de trabalho complexos e multietapas em sistemas distribuídos.
- Utiliza C# para fornecer um exemplo claro e conciso de implementação de Saga.
- Oferece flexibilidade para estender e personalizar Sagas com base em requisitos comerciais específicos.

## Requisitos
- .NET Core ou .NET Framework instalado no ambiente de desenvolvimento.
- Biblioteca MassTransit instalada via NuGet Package Manager.

## Componentes
- SagaService: Projeto de implementação e comportamento da Saga Orquestrada.
- PurchaseApi: Projeto API responsavel pela comunicação entre o cliente e aplicação, trata o recebimento e os eventos de notificação da "proposta".
- Reserve: Projeto responsavel por tratar os eventos de reserva de valores solicitados.
- Loan: Projeto responsavel por tratar os eventos de emprestimo de valores solicitados.
- Invoice: Projeto responsavel por tratar os eventos de fatura de valores solicitados.

## Uso
- Explore as Sagas fornecidas dentro do projeto.
- Personalize as Sagas ou crie novas para atender aos requisitos da sua aplicação.
- Execute o aplicativo e observe a execução da Saga em ação.

## Diagrama

![Diagrama](https://github.com/luis4ndre/MassTransitPOC/blob/7ba75afd1cc9e7cbf7f76c293ad8402a73fcdbdb/SAGA%20POC.drawio.png?raw=true)

## Topologia SQS/SNS
![Topologia](https://github.com/luis4ndre/MassTransitPOC/blob/7ba75afd1cc9e7cbf7f76c293ad8402a73fcdbdb/topologia.drawio.png?raw=true)
