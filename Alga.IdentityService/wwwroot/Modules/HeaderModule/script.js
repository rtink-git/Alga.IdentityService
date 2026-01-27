class HeaderModule extends BaseModule {
    constructor(id) {
        super(id);
        this._isDebug = document.URL.includes("://localhost");
    }

    push = (target, position = "afterend") => super.push(this._html(), target, position);

    _html() { const nm = `${this._logBase}_html()`; try {
        return `
<header id="${this.id}" class="${this.constructor.name}">
    <div>
        <a href="/">
            <p>I & S - Provider</p>
        </a>
    </div>
</header>`;
        } catch (error) { console.error(nm, error); }
    }
}