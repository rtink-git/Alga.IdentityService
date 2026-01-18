class SignInModule extends BaseModule {
    constructor(id) {
        super(id);
        this._isDebug = document.URL.includes("://localhost");
        this._url = this._isDebug ? "https://localhost:7051" : "https://api.rt.ink";
    }

    push (target, position = "afterend") {
        super.push(this._html(), target, position);

        document.querySelector(`#${this.id} #_GoogleB`).addEventListener('click', async () => {
            window.location.href = `${this._url}/auth/google/signin?redirect-uri=${this._url}`;
        });
    }

    _html() { const nm = `${this._logBase}_html()`; try {
        return `
<div id="${this.id}" class="${this.constructor.name}">
    <h1>Identity & Session Provider</h1>
    <button id="_GoogleB">
        <span>Continue with Google</span>
    </button>
</div>`;
        } catch (error) { console.error(nm, error); }
    }


}