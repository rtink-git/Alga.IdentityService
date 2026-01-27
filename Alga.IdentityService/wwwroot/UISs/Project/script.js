export async function ProjectUIS() {
    var sApi = new AlgaIdentityServiceAPI();
    var authF = await sApi.sessionRefresh()

    if(!authF) window.location.href = "/"

    // --------------------

    let headerModule = new HeaderModule();
    headerModule.push(document.getElementsByTagName("body")[0], "beforeend")

    // --------------------

    let lastVerticalId = headerModule.id

    // --------------------

    document.getElementById(lastVerticalId).insertAdjacentHTML("afterend", `<main></main>`)

    // --------------------

    var projectModel = await sApi.getProjectJson();

    if(!projectModel) location.href = "/"

    // --------------------

    let projectModule = new ProjectModule(null, projectModel.userId, projectModel.projectIds, sApi);
    projectModule.push(document.getElementsByTagName("main")[0], "beforeend")
}
