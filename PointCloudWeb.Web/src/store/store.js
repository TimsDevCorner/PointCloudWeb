import { createStore } from 'vuex'
import modulePCI from './pointCloudInfo/store.js'

export default createStore({
    namespaced: true,
    state: {
        dummy: "dummy"
    },
    modules: {
        pci: modulePCI
    }
})