pipeline {
    agent any

    environment {
      
    }

    stages {
        stage('Git Checkout') {
            steps {
                echo 'Pulling ... '
                git branch: 'main',
                url: 'https://github.com/skanderGrami/TestingSoft.git'
            }
        stage('Prepare Environment') {
            steps {
                script {
                    bat "xcopy /E /I /Y \"${env.PROJECT_PATH}\" \"${env.WORKSPACE}\""
                }
            }
        }

        stage('Install ReportGenerator') {
            steps {
                script {
                    bat 'dotnet tool install -g dotnet-reportgenerator-globaltool || exit 0'
                }
            }
        }

        stage('Build') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        bat 'dotnet restore'
                        bat 'dotnet build --configuration Release'
                    }
                }
            }
        }

        stage('Run Tests') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        // Créez manuellement le dossier results
                        bat 'mkdir results || exit 0'

                        // Exécutez les tests et affichez tous les logs
                        bat 'dotnet test --logger trx;LogFileName=results/TestResults.trx --results-directory results -v n'
                    }
                }
            }
        }

        stage('Verify Test Results') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        // Lister le contenu du dossier des résultats des tests
                        bat 'dir results'
                    }
                }
            }
        }

        stage('Generate HTML Report') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        // Utiliser le chemin correct basé sur l'étape précédente
                        bat 'reportgenerator "-reports:results/TestResults.trx" "-targetdir:TestResults" "-reporttypes:Html"'
                    }
                }
            }
        }

        stage('Publish Reports') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        publishHTML([allowMissing: false, alwaysLinkToLastBuild: true, keepAll: true, reportDir: 'TestResults', reportFiles: 'index.html', reportName: 'Test Report'])
                        archiveArtifacts artifacts: 'ReportsVideo/*.json, ReportsVideo/*.cs, ReportsVideo/*.mp4', allowEmptyArchive: true
                    }
                }
            }
        }
    }

    post {
        always {
            cleanWs()
        }
    }
}
