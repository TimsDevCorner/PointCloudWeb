import axios from 'axios'

export default {
    namespaced: true,
    strict: true,
    state: {
        pointClouds: [],
        loading: false
    },
    // getters: {
    //     pointClouds: state => state.pointClouds,
    //     loading: state => state.loading
    // },    
    mutations: {
        SET_POINT_CLOUDS(state, pointClouds) {
            state.pointClouds = pointClouds
        },
        SET_LOADING(state, loading) {
            state.loading = loading
        }
    },
    actions: {
        loadPointClouds({ commit }) {
            commit('SET_LOADING', true)
            axios
                .get('http://localhost:5000/pointcloudinfo')
                .then(response => {
                    commit('SET_POINT_CLOUDS', response.data)
                    commit('SET_LOADING', false)
                })
        }
    }
}