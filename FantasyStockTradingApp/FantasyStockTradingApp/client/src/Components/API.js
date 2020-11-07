﻿import axios from 'axios';

axios.defaults.headers.post['Content-Type'] = 'application/json';

const api = axios.create({
    baseURL: '/api/fantasystocktrading/',
});

/*const iexCloudAPI = axios.create({
    baseURL: '/api/fantasystocktrading/',
});*/

export {
    api
};