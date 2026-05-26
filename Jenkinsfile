pipeline {
    agent any

    environment {
        GIT_REPO_URL = 'https://github.com/DigvijayKamble/CICDDemo.git'
        GIT_BRANCH   = 'main'

        APP_POOL_NAME = 'DefaultAppPool'
        DEPLOY_PATH   = 'C:\\inetpub\\wwwroot\\my-dotnet-app'

        IMAGE_NAME    = 'mysampleapi'
        IMAGE_TAG     = 'v1'
    }

    stages {

        stage('Checkout Source') {
            steps {

                echo "Cleaning workspace..."
                cleanWs()

                echo "Fetching latest code from GitHub..."

                git url: "${GIT_REPO_URL}", branch: "${GIT_BRANCH}"
            }
        }

        stage('Restore Packages') {
            steps {

                echo 'Restoring NuGet packages...'

                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {

                echo 'Building application...'

                bat 'dotnet build --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {

                echo 'Running automated tests...'

                bat 'dotnet test --configuration Release --no-build'
            }
        }

        stage('Publish') {
            steps {

                echo 'Publishing application...'

                bat 'dotnet publish --configuration Release --output .\\publish --no-build'
            }
        }

        stage('Docker Build') {
            steps {

                echo 'Building Docker image...'

                bat 'docker build -t %IMAGE_NAME%:%IMAGE_TAG% .'
            }
        }

        stage('Deploy to IIS') {
            steps {

                echo 'Stopping IIS App Pool...'

                powershell '''
                    Import-Module WebAdministration
                    Stop-WebAppPool "${env:APP_POOL_NAME}"
                '''

                echo 'Deploying application files...'

                bat 'robocopy .\\publish\\ "%DEPLOY_PATH%" /MIR'

                echo 'Starting IIS App Pool...'

               powershell '''
                    Import-Module WebAdministration
                    Start-WebAppPool "DefaultAppPool"
                '''
            }
        }

        stage('Run Docker Container') {
            steps {

                echo 'Removing old container if exists...'

                bat 'docker rm -f mysamplecontainer || exit 0'

                echo 'Running Docker container...'

                bat 'docker run -d -p 9090:80 --name mysamplecontainer %IMAGE_NAME%:%IMAGE_TAG%'
            }
        }
    }

    post {

        success {
            echo 'CI/CD Pipeline executed successfully!'
        }

        failure {
            echo 'Pipeline failed! Check console logs.'
        }

        always {
            echo 'Pipeline execution completed.'
        }
    }
}