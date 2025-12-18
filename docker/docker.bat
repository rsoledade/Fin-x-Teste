@echo off
REM Script auxiliar para gerenciar o ambiente Docker no Windows

setlocal

cd /d "%~dp0"

if "%1"=="" goto help
if "%1"=="start" goto start
if "%1"=="stop" goto stop
if "%1"=="restart" goto restart
if "%1"=="logs" goto logs
if "%1"=="build" goto build
if "%1"=="clean" goto clean
if "%1"=="status" goto status
if "%1"=="help" goto help
goto help

:check_env
    if not exist ".env" (
        echo [WARN] Arquivo .env nao encontrado.
        if exist ".env.example" (
            copy .env.example .env
            echo [INFO] Arquivo .env criado. Por favor, revise as configuracoes.
            exit /b 1
        ) else (
            echo [ERROR] Arquivo .env.example nao encontrado!
            exit /b 1
        )
    )
    exit /b 0

:start
    echo [INFO] Iniciando servicos...
    call :check_env
    if errorlevel 1 exit /b 1
    docker-compose up -d
    echo [INFO] Servicos iniciados!
    echo [INFO] API disponivel em: http://localhost:5000
    echo [INFO] Health check: http://localhost:5000/health/ready
    goto end

:stop
    echo [INFO] Parando servicos...
    docker-compose down
    echo [INFO] Servicos parados!
    goto end

:restart
    call :stop
    call :start
    goto end

:logs
    if "%2"=="" (
        docker-compose logs -f api
    ) else (
        docker-compose logs -f %2
    )
    goto end

:build
    echo [INFO] Rebuilding API...
    call :check_env
    if errorlevel 1 exit /b 1
    docker-compose build --no-cache api
    echo [INFO] Build completo!
    goto end

:clean
    echo [WARN] Isso ira remover todos os containers, volumes e dados. Continuar? (S/N)
    set /p response=
    if /i "%response%"=="S" (
        echo [INFO] Limpando ambiente...
        docker-compose down -v
        echo [INFO] Ambiente limpo!
    ) else (
        echo [INFO] Operacao cancelada.
    )
    goto end

:status
    docker-compose ps
    goto end

:help
    echo Uso: docker.bat [comando]
    echo.
    echo Comandos disponiveis:
    echo   start     - Inicia todos os servicos em background
    echo   stop      - Para todos os servicos
    echo   restart   - Reinicia todos os servicos
    echo   logs      - Mostra logs (padrao: api). Use: docker.bat logs [servico]
    echo   build     - Rebuild da API sem cache
    echo   clean     - Remove todos os containers e volumes (CUIDADO!)
    echo   status    - Mostra status dos containers
    echo   help      - Mostra esta mensagem
    echo.
    echo Exemplos:
    echo   docker.bat start
    echo   docker.bat logs api
    echo   docker.bat logs sqlserver
    echo   docker.bat build
    goto end

:end
endlocal
