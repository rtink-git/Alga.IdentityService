class ProjectModule extends BaseModule {
    constructor(id, userId, projectIds, algaIdentityServiceAPI) {
        super(id);
        this._isDebug = document.URL.includes("://localhost");
        this._userId = userId;
        this._projectIds = projectIds
        this._algaIdentityServiceAPI = algaIdentityServiceAPI
    }

    push (target, position = "afterend") {
        super.push(this._html(), target, position);

        var createBT = document.querySelector(`#${this.id} ._create button`)
        if(createBT)
            createBT.addEventListener("click", async (e) => {
                var newProject = await this._algaIdentityServiceAPI.getProjectCreateJson();
                console.log(newProject)
                if (newProject) {
                    document.querySelector(`#${this.id} ._create`).remove();
                    document.querySelector(`#${this.id}`).insertAdjacentHTML("beforeend", this._project_form_html(newProject.projectId))
                }   
            });
        var secretKeyBT = document.querySelector(`#${this.id} ._project button`)
        if(secretKeyBT)
            secretKeyBT.addEventListener("click", async (e) => {
                let projectId = e.target.closest("._project").getAttribute("data-projectId")
                if(projectId) {
                    var projectSecretKey = await this._algaIdentityServiceAPI.getProjectSecretKeyJson(projectId);
                    console.log(projectSecretKey)
                    try { await navigator.clipboard.writeText(projectSecretKey.secretKey); } catch (err) {}
                }
            });
    }

    _html() { const nm = `${this._logBase}_html()`; try {
        var formHtml = "";
        if(!this._projectIds || this._projectIds.length == 0) {
            formHtml = this._project_create_form_html()
        }
        else this._projectIds.forEach(el => { formHtml += this._project_form_html(el); });

        return `
<div id="${this.id}" class="${this.constructor.name}">
    <h1>
        Project Provider
    </h1>
    <p class="_user">
        User Id: <span>${this._userId}</span>
    </p>
    <p class="_description">
        A unique identifier for using a unified authentication service across all your projects.
    </p>
    ${formHtml}
</div>`;
    } catch (error) { console.error(nm, error); }}

    _project_create_form_html() {
         return `
<div class="_create _form">
    <button>
        <span>
            + Create Project Provider
        </span>
    </button>
</div>
`
    }

    _project_form_html(projectId){ const nm = `${this._logBase}_project_form_html()`; try {
         return `
<div class="_project _form" data-projectId="${projectId}">
    <div>
        <span>
            ${projectId}
        </span>
    </div>
    <button>
        <span>
            Get New Secret Key
        </span>
    </button>
</div>
`
    } catch (error) { console.error(nm, error); }}
}