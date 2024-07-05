pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/skanderGrami/TestingSoft.git'
            }
        }
        stage('Build') {
            steps {
                bat 'msbuild YourProject.sln /p:Configuration=Release'
            }
        }
        stage('Test') {
            steps {
                bat 'nunit3-console YourTestProject.dll --result=TestResult.xml'
            }
        }
        stage('Publish Test Results') {
            steps {
                nunit testResultsPattern: 'TestResult.xml'
            }
        }
        stage('Quality Analysis') {
            steps {
                script {
                    // Ajoutez ici les étapes pour analyser la qualité du code avec SonarQube ou d'autres outils
                    echo "Analyse de la qualité du code en cours..."
                }
            }
        }
    }

    post {
        always {
            archiveArtifacts artifacts: '**/bin/**', allowEmptyArchive: true
        }
        failure {
            mail to: 'skandergrami@gmail.com',
                 subject: "Build failed in Jenkins: ${currentBuild.fullDisplayName}",
                 body: "Check the Jenkins job for details: ${env.BUILD_URL}"
        }
    }
}
