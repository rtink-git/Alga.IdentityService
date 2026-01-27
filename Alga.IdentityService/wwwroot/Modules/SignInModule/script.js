class SignInModule extends BaseModule {
    constructor(id) {
        super(id);
        this._isDebug = document.URL.includes("://localhost");
        this._url = this._isDebug ? "https://localhost:7051" : "https://auth.rt.ink";
        const url = new URL(document.URL);
        this._projectId = url.searchParams.get("project-id") ?? "10c6782d-cad5-47ce-b8e1-90713b702834";
        this._baseUrl = url.searchParams.get("base-url") ?? (this._isDebug ? "https://localhost:7051" : "https://auth.rt.ink");
    }

    push (target, position = "afterend") {
        super.push(this._html(), target, position);

        document.querySelector(`#${this.id} #_GoogleB`).addEventListener('click', async () => {
            window.location.href = `${this._url}/google/signin?project-id=${this._projectId}&base-url=${this._baseUrl}`;
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