export async function HomeUIS() {
    document.getElementsByTagName("body")[0].insertAdjacentHTML("afterbegin", `<main></main>`)
    
    // --------------------

    let signInModule = new SignInModule();
    signInModule.push(document.getElementsByTagName("main")[0], "beforeend")

    // --------------------

    let lastVerticalId = signInModule.id
}