# TimeclockBot

### Sobre

Aplicação desenvolvida para a automação do processo de ponto dos sistemas Senior-X.

### Como utilizar

Crie as seguintes variáveis de ambiente no seu Azure e no seu computador de desenvolvimento:

```
SENIOR_USER=[seu usuário]
SENIOR_PASSWORD=[sua senha]
SENIOR_SUFFIX=[sufixo do seu email empresarial]
SENIOR_LAT=[latitude da sua localização, em decimais]
SENIOR_LNG=[longitude da sua localização, em decimais]
WEBSITE_TIME_ZONE=E. South America Standard Time
```

Depois disso é só deployar manualmente ou através do botão abaixo:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fteofilosalgado%2FTimeclockBot%2Fmaster%2Ftemplate.json)
