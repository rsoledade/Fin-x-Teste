#!/bin/bash
# Script auxiliar para gerenciar o ambiente Docker

set -e

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd "$SCRIPT_DIR"

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

function print_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

function print_warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

function print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

function check_env() {
    if [ ! -f ".env" ]; then
        print_warn "Arquivo .env não encontrado. Copiando de .env.example..."
        if [ -f ".env.example" ]; then
            cp .env.example .env
            print_info "Arquivo .env criado. Por favor, revise as configurações antes de continuar."
            exit 0
        else
            print_error "Arquivo .env.example não encontrado!"
            exit 1
        fi
    fi
}

function start() {
    print_info "Iniciando serviços..."
    check_env
    docker-compose up -d
    print_info "Serviços iniciados!"
    print_info "API disponível em: http://localhost:5000"
    print_info "Health check: http://localhost:5000/health/ready"
}

function stop() {
    print_info "Parando serviços..."
    docker-compose down
    print_info "Serviços parados!"
}

function restart() {
    stop
    start
}

function logs() {
    docker-compose logs -f "${2:-api}"
}

function build() {
    print_info "Rebuilding API..."
    check_env
    docker-compose build --no-cache api
    print_info "Build completo!"
}

function clean() {
    print_warn "Isso irá remover todos os containers, volumes e dados. Continuar? (y/N)"
    read -r response
    if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
        print_info "Limpando ambiente..."
        docker-compose down -v
        print_info "Ambiente limpo!"
    else
        print_info "Operação cancelada."
    fi
}

function status() {
    docker-compose ps
}

function help() {
    cat << EOF
Uso: ./docker.sh [comando]

Comandos disponíveis:
  start     - Inicia todos os serviços em background
  stop      - Para todos os serviços
  restart   - Reinicia todos os serviços
  logs      - Mostra logs (padrão: api). Use: ./docker.sh logs [serviço]
  build     - Rebuild da API sem cache
  clean     - Remove todos os containers e volumes (CUIDADO!)
  status    - Mostra status dos containers
  help      - Mostra esta mensagem

Exemplos:
  ./docker.sh start
  ./docker.sh logs api
  ./docker.sh logs sqlserver
  ./docker.sh build
EOF
}

case "${1:-help}" in
    start)
        start
        ;;
    stop)
        stop
        ;;
    restart)
        restart
        ;;
    logs)
        logs "$@"
        ;;
    build)
        build
        ;;
    clean)
        clean
        ;;
    status)
        status
        ;;
    help|*)
        help
        ;;
esac
