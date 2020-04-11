# NetCore - Desafio 01

Para executar este projeto em seu potencial é necessário ter o .Net Core 3.1 rodando na máquina.

#### Dependencias
Todas as dependencias podem ser opcionalmente instaladas através do [Chocolatey](https://chocolatey.org/).

##### .Net Core
* Obtenha o .Net Core pelo:
 * [Site oficial](https://dotnet.microsoft.com/download), ou use
 
        > choco install dotnetcore
 
##### Git
* Obtenha o git pelo:
 * [Site oficial](https://git-for-windows.github.io/), ou use:

        > choco install git
 
#### Baixando o Projeto

O download do projeto pode ser feito através do `git` seguindo os passos:

    -aponte para a pasta da solução
     > git clone https://github.com/hugoestevam/netcore-desafio-01.git

#### Preparando o ambiente

##### Resumo dos comandos
    
-Você pode executar os comandos do .Net Core através de um terminal ou prompt de comando.    
        
-Na pasta do projeto, rode esse comando para executar a instalação dos pacotes:

    > dotnet restore         
        
-Execute esse comando para subir o servidor Kestrel.

    > dotnet run

-Para executar os testes através do xUnit, basta rodar o comando abaixo:

    > dotnet test


##### Detalhes

* Se estiver usando Windows, inicie o prompt com permissões de administrador para que **choco install** funcione corretamente.
* Ao rodar o comando **dotnet run** o Kestrel vai executar uma aplicação Console que ficará registrando o log da API.
* Se tudo estiver ok, o prompt irá listar todas as saídas abaixo.

```shell
    $ dotnet run
    Copyright (c) Microsoft Corporation.  All rights reserved.
    info: Microsoft.Hosting.Lifetime[0]
        Now listening on: http://localhost:5000
    info: Microsoft.Hosting.Lifetime[0]
        Now listening on: https://localhost:5001
    info: Microsoft.Hosting.Lifetime[0]
        Application started. Press Ctrl+C to shut down.
```
#### Executando
Basicamente deve estar executando a API do .Net Core. A aplicação já deve estar executando em  [http://localhost:5000](http://localhost:5000).

* [Insomia](https://insomnia.rest/)
    * Você pode importar o arquivo **insomnia-desafio-01.json** que se encontra na pasta principal do projeto. Nele já existem todas as chamadas para a API.
 * [xUnit](https://xunit.net/)
    * Você pode executar os testes através do comando `> dotnet test`;

```shell
$ dotnet test
Microsoft (R) Test Execution Command Line Tool Version 16.5.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.

Test Run Successful.
Total tests: 9
     Passed: 9
 Total time: 2.4096 Seconds
```

## FIM

Qualquer dúvida entre em contato comigo!
Obrigado.