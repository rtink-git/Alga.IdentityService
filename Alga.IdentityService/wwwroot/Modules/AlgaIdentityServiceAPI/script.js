class AlgaIdentityServiceAPI {
    constructor() {
        this._logBase = `Page component: ${this.constructor.name}.`
        this._isDebug = document.URL.includes("://localhost");
        if(this._isDebug) console.debug(this._logBase);
        this._urlApi = this._isDebug ? "https://localhost:7051" : "https://auth.rt.ink";

        this._algaSessionLS = "AlgaIdentityServiceSession"
        if(this._isDebug) console.debug(this._LogBase + " Ready to use")
        this._sessionCache = localStorage.getItem(this._algaSessionLS) || null;
    } 

    setSessionToCache(session) {
        if (!session) return false;
        this._sessionCache = session;
        localStorage.setItem(this._algaSessionLS, session);
        return true;
    }

    removeSession() { this._sessionCache = null; localStorage.removeItem(this._algaSessionLS); }

    async sessionRefresh() {
        try {
            const response = await this._R(`/refresh?session=${this._sessionCache}`, true);
            this.setSessionToCache(await response.json());

            return response.ok
        } catch { 
            this.removeSession(); 
            return false;
        }
    }

    async sessionRefreshAndRedirect (redirectUrl) {
        if(await this.sessionRefresh()) window.location.href = redirectUrl ? redirectUrl : `${this._urlApi}/project`
        else window.location.href = redirectUrl ? redirectUrl : `${this._urlApi}/error/session`
    }

    async getProjectJson() { return await this._R_JSON('/api/project', true); }
    async getProjectCreateJson() { return await this._R_JSON('/api/project/create', true); }
    async getProjectSecretKeyJson(projectId) { return await this._R_JSON(`/api/project/secret-key/new?projectId=${projectId}`, true); }

    async _R_JSON(urlPart, isAuth = false, cacheInSec = 0, method = 'GET', headers = {}) {
        var response = await this._R(urlPart, isAuth, cacheInSec, method, method)
        if(response.ok) return await response.json();
        else return null;
    }

    async _R(urlPart, isAuth = false, cacheInSec = 0, method = 'GET', headers = {}) { try {
        const requestHeaders = { ...headers, ...(cacheInSec > 0 && { 'Cache-Control': `public, max-age=${cacheInSec}` }), ...(isAuth && this._sessionCache && { [this._algaSessionLS]: this._sessionCache }) };
        const response = await fetch(`${this._urlApi}${urlPart}`, { method, headers: requestHeaders });
        if (response?.ok) {
            return response;
        }
        throw new Error(`HTTP ${response.status}: ${await response.text()}`);
    } catch (error) { this._logError(`_R ${urlPart}`, error); return null; }}

    _logError(...args) { console.error(this._logBase, ...args); }
}

