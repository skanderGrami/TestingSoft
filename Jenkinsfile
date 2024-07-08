pipeline {
    agent any

    environment {
        PROJECT_PATH = 'E:/StagePFE_TriWeb/PlatformProject/TestingSoft_Backend/TestingSoft_Backend'
    }

    stages {
        stage('Git Checkout') {
            steps {
                echo 'Pulling from Git repository...'
                git branch: 'main', url: 'https://github.com/skanderGrami/TestingSoft.git'
            }
        }

        stage('Prepare Environment') {
            steps {
                script {
                    // Copier le projet depuis PROJECT_PATH vers le WORKSPACE
                    bat "xcopy /E /I /Y \"${env.PROJECT_PATH}\" \"${env.WORKSPACE}\""
                }
            }
        }

        stage('Install ReportGenerator') {
            steps {
                script {
                    // Installer ReportGenerator si nécessaire
                    bat 'dotnet tool install -g dotnet-reportgenerator-globaltool || exit 0'
                }
            }
        }

        stage('Build') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        // Restaurer les dépendances et construire le projet
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
                        // Créer le dossier 'results' s'il n'existe pas déjà
                        bat 'mkdir results || exit 0'

                        // Exécuter les tests et générer le fichier TRX
                        bat 'dotnet test --logger trx;LogFileName=results/TestResults.trx --results-directory results -v n'
                    }
                }
            }
        }

        stage('Verify Test Results') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        // Vérifier si le fichier TRX a été correctement généré
                        bat 'dir results'
                    }
                }
            }
        }

        stage('Generate HTML Report') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        // Utiliser ReportGenerator pour générer le rapport HTML à partir du fichier TRX
                        bat 'reportgenerator "-reports:results/TestResults.trx" "-targetdir:TestResults" "-reporttypes:Html"'
                    }
                }
            }
        }

        stage('Publish Reports') {
            steps {
                dir("${env.WORKSPACE}") {
                    script {
                        // Publier les rapports HTML générés
                        publishHTML([allowMissing: false, alwaysLinkToLastBuild: true, keepAll: true, reportDir: 'TestResults', reportFiles: 'index.html', reportName: 'Test Report'])

                        // Archiver d'autres artefacts comme les fichiers vidéo ou JSON
                        archiveArtifacts artifacts: 'ReportsVideo/*.json, ReportsVideo/*.cs, ReportsVideo/*.mp4', allowEmptyArchive: true
                    }
                }
            }
        }
    }

    post {
        always {
            cleanWs()  // Nettoyer l'espace de travail après chaque exécution
        }
    }
}
