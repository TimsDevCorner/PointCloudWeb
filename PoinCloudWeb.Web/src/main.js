import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faEdit, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(faEdit, faEye, faEyeSlash)


//Vue.config.productionTip = false

const app = createApp(App)
    .use(router)
    .component('font-awesome-icon', FontAwesomeIcon);

app.mount('#app')
