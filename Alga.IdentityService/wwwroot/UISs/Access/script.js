export async function AccessUIS() {
    document.getElementsByTagName("body")[0].insertAdjacentHTML("afterbegin", `<main></main>`)
    
    // --------------------

    let accessModule = new AccessModule();
    accessModule.push(document.getElementsByTagName("main")[0], "beforeend")

    // --------------------

    var sApi = new AlgaIdentityServiceAPI();
    var session = window.location.search.replace('?session=', '');
    sApi.setSessionToCache(session)
    await sApi.sessionRefreshAndRedirect()
}