pipeline {
    agent any

    stages {
        stage('Git Checkout') {
            steps {
                echo 'Pulling ... '
                git branch: 'main', url: 'https://github.com/skanderGrami/TestingSoft.git'
            }
        }

        stage('Prepare Environment') {
            steps {
                script {
                    // Crée le dossier results s'il n'existe pas
                    bat 'mkdir results || exit 0'
                    // Copie les fichiers du projet dans le dossier de travail
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
                        // Exécute les tests et génère le fichier TRX dans results
                        bat 'dotnet test --logger trx;LogFileName=results/TestResults.trx --results-directory results -v n'
                    }
                }
            }
        }

        stage('Generate HTML Report') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        // Génère le rapport HTML à partir du fichier TRX
                        bat 'reportgenerator "-reports:results/TestResults.trx" "-targetdir:TestResults" "-reporttypes:Html"'
                    }
                }
            }
        }

        stage('Publish Reports') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        // Publie les rapports HTML générés
                        publishHTML([allowMissing: false, alwaysLinkToLastBuild: true, keepAll: true, reportDir: 'TestResults', reportFiles: 'index.html', reportName: 'Test Report'])
                        // Archive les artefacts comme les fichiers JSON et MP4
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
