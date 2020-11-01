import axios from 'axios';

axios.defaults.headers.post['Content-Type'] = 'application/json';

const dbAccess = axios.create({
    baseURL: 'https://localhost:5001/api/fantasystocktrading/',
});

const iexCloudAPI = axios.create({
    baseURL: 'https://cloud.iexapis.com/v1/',
});

export {
    dbAccess,
    iexCloudAPI
};