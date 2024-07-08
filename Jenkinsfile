pipeline {
    agent any
    
    stages {
        stage('Checkout') {
            steps {
                // Checkout the repository
                git 'https://github.com/skanderGrami/TestingSoft.git'
            }
        }
        
        stage('Build') {
            steps {
                // Example build step, replace with your build commands
                sh 'dotnet build'
            }
        }
        
        stage('Test') {
            steps {
                // Example test step, replace with your test commands
                sh 'dotnet test'
            }
            
            post {
                always {
                    // Archive test results
                    junit '**/TestResult.xml'
                }
            }
        }
        
        stage('Generate Reports') {
            steps {
                // Example step to generate test reports
                // Replace this with your report generation commands
                sh 'dotnet report-generator'
            }
            
            post {
                success {
                    // Publish the generated reports
                    archiveArtifacts artifacts: 'reports/**/*.html', allowEmptyArchive: true
                }
            }
        }
    }
    
    // Post pipeline cleanup
    post {
        always {
            // Delete 'results' folder to clean up
            cleanWs()
        }
    }
}
