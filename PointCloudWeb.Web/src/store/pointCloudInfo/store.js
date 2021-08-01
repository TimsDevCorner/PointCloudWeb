import axios from 'axios'
import globals from '@/globals.js'

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
        },
        UPDATE_PC(state, pointCloud) {
            const index = state.pointClouds.findIndex(x => x.id === pointCloud.data.id);
            if (index === -1)
                return;

            if (pointCloud.action === "update") {
                state.pointClouds[index] = pointCloud.data;
            }
            else if (pointCloud.action === "remove") {
                state.pointClouds.splice(index, 1);
            }
        },
    },
    actions: {
        loadPointClouds({ commit }) {
            commit('SET_LOADING', true)
            axios
                .get(globals.API_URL + 'pointcloudinfo')
                .then(response => {
                    commit('SET_POINT_CLOUDS', response.data);
                    commit('SET_LOADING', false);
                }).catch(() => {
                    commit('SET_LOADING', false);
                })
        },
        updatePointCloud({ commit }, pointCloudInfo) {
            commit('SET_LOADING', true);
            axios
                .put(globals.API_URL + 'pointcloudinfo', pointCloudInfo)
                .then(response => {
                     setTimeout(() => {
                        commit('UPDATE_PC', { action: "update", data: response.data });
                        commit('SET_LOADING', false);
                    }, 80);
                }).catch(() => {
                    commit('SET_LOADING', false)
                })
        },
        deletePointCloud({ commit }, pointCloudInfo) {
            commit('SET_LOADING', true);
            axios
                .delete(globals.API_URL + 'pointcloudinfo/' + pointCloudInfo.id)
                .then(() => {
                    commit('UPDATE_PC', { action: "remove", data: pointCloudInfo });
                    commit('SET_LOADING', false);
                }).catch(e => {
                    alert(e);
                    commit('SET_LOADING', false);
                })
        }
    }
}
