class AccessModule extends BaseModule {
    constructor(id) {
        super(id);
        this._isDebug = document.URL.includes("://localhost");
        // this._url = this._isDebug ? "https://localhost:7051" : "https://api.rt.ink";
    }

    push = (target, position = "afterend") => super.push(this._html(), target, position);

    _html() { const nm = `${this._logBase}_html()`; try {
        return `
<div id="${this.id}" class="${this.constructor.name}">
    <a href="/">
        <span class="_loader"></span>
    </a>
</div>`;
        } catch (error) { console.error(nm, error); }
    }
}