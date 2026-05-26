pipeline {
    agent any

    environment {
        // Define your Git repository URL and branch
        GIT_REPO_URL  = 'https://github.com/DigvijayKamble/CICDDemo.git'
        GIT_BRANCH    = 'main'
        
        // Define your Jenkins Credentials ID for Git (if the repo is private)
        // Create this under Manage Jenkins -> Credentials
        //GIT_CREDS_ID  = 'github-credentials-id' 
    }

    stages {
        stage('Checkout Source') {
            steps {
                  cleanWs()
                echo "Fetching latest code from ${GIT_REPO_URL} [${GIT_BRANCH}]..."
                
                // For Public Repos (No credentials needed):
                git url: "${GIT_REPO_URL}", branch: "${GIT_BRANCH}"
                
                // For Private Repos (Uncomment below and comment out the public line above):
                // checkout scmGit(branches: [[name: "${GIT_BRANCH}"]], userRemoteConfigs: [[credentialsId: "${GIT_CREDS_ID}", url: "${GIT_REPO_URL}"]])
            }
        }

        stage('Build & Test') {
            steps {
                 echo 'Restoring NuGet packages...'
                 bat 'dotnet restore'

                 echo 'Building the project in Release mode...'
                 bat 'dotnet build --configuration Release --no-restore'
        
                 echo 'Running automated tests...'
                 bat 'dotnet test --configuration Release --no-build'
            }
        }

        stage('Deploy Application') {
            steps {
                echo 'Publishing the application...'
                // Compiles and outputs self-contained files to a local 'publish' folder
                bat 'dotnet publish --configuration Release --output ./publish --no-build'
                
                echo 'Deploying published files to target directory...'
                // Copies the compiled files to your local Windows server folder (e.g., IIS)
                bat 'xcopy /E /Y .\\publish\\* "C:\\inetpub\\wwwroot\\my-dotnet-app\\"'
            }
        }
    }

    post {
        success {
            echo 'Deployment completed successfully!'
        }
        failure {
            echo 'Pipeline failed. Check the console output above to debug errors.'
        }
    }
}
