import axios from 'axios';

axios.defaults.headers.post['Content-Type'] = 'application/json';

const api = axios.create({
    baseURL: '/api/fantasystocktrading/',
});

const tokenApi = axios.create({
    baseURL: '/api/token/',
});

/*const iexCloudAPI = axios.create({
    baseURL: '/api/fantasystocktrading/',
});*/

export {
    api,
    tokenApi
};