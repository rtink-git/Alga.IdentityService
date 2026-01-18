class BaseModule {
    constructor(id) {
        this._logBase = `Page component: ${this.constructor.name}.`
        this.id = id && id?.length > 0 ? id : this.constructor.name;
        this._logBase += this.id != this.constructor.name ? ` id: ${this.id}.` : ``;
        this._isDebug = location.hostname === "localhost";
        if(this._isDebug) console.debug(this._logBase)
    }

    push(html, target, position = "afterend") {
        const nm = `${this._logBase}push()`;
        try {
            target = typeof target === "object" ? target : document.getElementById(target);
            if (html?.length > 0) target.insertAdjacentHTML(position, html);
            if (this._isDebug) console.debug(nm);
        } catch (error) { console.error(nm, error); }
    }
}