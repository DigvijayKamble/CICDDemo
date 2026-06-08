pipeline {
    agent any

    environment {
        GIT_REPO_URL = 'https://github.com/DigvijayKamble/CICDDemo.git'
        GIT_BRANCH   = 'main'

        IMAGE_NAME   = 'mysampleapi'
        IMAGE_TAG    = 'v1'
        CONTAINER_NAME = 'mysamplecontainer'
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

        stage('Docker Build') {
            steps {
                echo 'Building Docker image...'
                bat 'docker build -t %IMAGE_NAME%:%IMAGE_TAG% .'
            }
        }

        stage('Docker Deploy') {
            steps {
                echo 'Removing old container if exists...'

                bat '''
                docker stop %CONTAINER_NAME%
                docker rm %CONTAINER_NAME%
                exit /b 0
                '''

                echo 'Starting new container...'

                bat 'docker run -d -p 9090:80 --name %CONTAINER_NAME% %IMAGE_NAME%:%IMAGE_TAG%'
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