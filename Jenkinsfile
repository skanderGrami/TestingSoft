pipeline {
    agent any
    
    stages {
        stage('Checkout') {
            steps {
                // Checkout the repository
                git branch: 'main', url: 'https://github.com/skanderGrami/TestingSoft.git'
            }
        }
        
        stage('Build') {
            steps {
                bat 'dotnet restore' // Restauration des packages .NET Core
                bat 'dotnet build'   // Construction du projet
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test --filter Category!=ExcludeFromTests' // Exécute les tests NUnit, exclut les tests marqués Category: ExcludeFromTests
            }
        }

        stage('Generate Reports') {
            steps {
                // Éventuellement, générer des rapports ici
                 sh 'dotnet report-generator'
            }
        }
    }

    post {
        always {
            junit '**/TestResult.xml' // Spécifiez l'emplacement de vos résultats de test NUnit
        }
    }
}
