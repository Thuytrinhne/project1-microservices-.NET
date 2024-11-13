pipeline {
    agent any
    environment{
        registry = "minhtuanhth95"
        registryCredential = "dockerhub"
        DOCKER_REGISTRY="minhtuanhth95"
        dockerImage = ""
        
        REGISTRY_NAME = "crkubercloud"
        ACR_LOGIN_SERVER = "${REGISTRY_NAME}.azurecr.io"
        REPOSITORY_NAME = "node-express-app"
    }
    stages { 
        // stage("Checkout"){
        //     steps{
        //         checkout scmGit(branches: [[name: '*/main']], extensions: [], userRemoteConfigs: [[credentialsId: 'justForPersonalTokenGithub', url: 'https://github.com/tuanhth95/project1-microservices-.NET-main.git']])
        //     }
        // }
        stage("check DOCKER_REGISTRY"){
            steps{
                script{
                    echo "DOCKER_REGISTRY is set to: ${DOCKER_REGISTRY}"
                }
            }
        }
        stage("Build docker image"){
            steps{
                script{
                    withEnv(["DOCKER_REGISTRY=${DOCKER_REGISTRY}"]){
                        sh '''
                        cd ./src
                        sudo DOCKER_REGISTRY=${ACR_LOGIN_SERVER}/${DOCKER_REGISTRY} docker-compose down
                        sudo DOCKER_REGISTRY=${ACR_LOGIN_SERVER}/${DOCKER_REGISTRY} docker-compose build
                        '''
                    }
                }
            }
        }
        stage('Upload Image to ACR') {
            steps{   
                script {
                    withCredentials([usernamePassword(credentialsId: 'crKuberCloud', usernameVariable: 'SERVICE_PRINCIPAL_ID', passwordVariable: 'SERVICE_PRINCIPAL_PASSWORD')]) {
                        withEnv(["DOCKER_REGISTRY=${DOCKER_REGISTRY}"]){
                            sh '''
                            cd ./src
                            docker login ${ACR_LOGIN_SERVER} -u $SERVICE_PRINCIPAL_ID -p $SERVICE_PRINCIPAL_PASSWORD
                            DOCKER_REGISTRY=${ACR_LOGIN_SERVER}/${DOCKER_REGISTRY} docker-compose push
                            '''
                        }
                    }
                }
            }
        }
    }
}