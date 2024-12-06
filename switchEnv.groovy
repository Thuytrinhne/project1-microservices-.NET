#!/usr/bin/env groovy

def switchEnv(environment) {
    def chartURL = "charts/ingressrule"

    if (!environment) {
        echo "Environment is required"
        error("No environment specified")
    }

    echo "Switching to ${environment} environment"

    def uninstallStatus = sh(script: "helm uninstall ingressrule", returnStatus: true)

    if (uninstallStatus == 0) {
        echo "Ingress deleted successfully"
    } else {
        echo "Not found ingressrule release"
    }

    sh "helm install ingressrule ${chartURL} --set namespace=${environment}"
}

return this
