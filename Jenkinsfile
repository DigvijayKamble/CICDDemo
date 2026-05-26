pipeline {
    agent any

    environment {
        GIT_REPO_URL = 'https://github.com/DigvijayKamble/CICDDemo.git'
        GIT_BRANCH   = 'main'
    }

    stages {

        stage('Checkout Source') {
            steps {
                echo "Cleaning workspace..."
                cleanWs()

                echo "Fetching latest code..."
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
                echo 'Running tests...'
                bat 'dotnet test --configuration Release --no-build'
            }
        }

        stage('Publish') {
            steps {
                echo 'Publishing application...'
                bat 'dotnet publish --configuration Release --output .\\publish --no-build'
            }
        }

        stage('Deploy') {
            steps {

                echo 'Stopping IIS App Pool...'
                bat 'powershell Stop-WebAppPool DefaultAppPool'

                echo 'Deploying files...'
                bat 'robocopy .\\publish\\ C:\\inetpub\\wwwroot\\my-dotnet-app\\ /MIR'

                echo 'Starting IIS App Pool...'
                bat 'powershell Start-WebAppPool DefaultAppPool'
            }
        }
    }

    post {
        success {
            echo 'CI/CD Pipeline executed successfully!'
        }

        failure {
            echo 'Pipeline failed!'
        }

        always {
            echo 'Pipeline execution completed.'
        }
    }
}