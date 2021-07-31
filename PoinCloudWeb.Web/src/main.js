import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faEdit, faEye, faEyeSlash, faTrash, faTimes } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import store from "./store/store.js"

library.add(faEdit, faEye, faEyeSlash, faTrash, faTimes)


//Vue.config.productionTip = false

const app = createApp(App)
    .use(router)
    .use(store)
    .component('font-awesome-icon', FontAwesomeIcon);

app.mount('#app')
