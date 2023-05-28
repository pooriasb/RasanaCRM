import axios from 'axios';

function appConnection(url, type, data) {
    return axios[type](url, data).then(function (result) {
        return result;
    }, function (error) {
        return error;
    });
}

const serverResponseHandle = function (deferredObj) { return deferredObj.then(function (result) { return result; }, function (message) { return message; }); };

const routes = {
    'get-product': { url: '/SellFactor/GetProduct', type: 'post' },
    'get-search-Customer': { url: '/SellFactor/GetCustomer', type: 'post' },
    'get-code': { url: '/SellFactor/Code', type: 'post' },
    'initialize-requirement': { url: '/SellFactor/Initionalize', type: 'get' },
    'add-factor-item': { url: '/SellFactor/FactorItem', type: 'post' },
    'remove-factor-item': { url: '/SellFactor/RemoveFactorItem', type: 'post' },
    'add-main-factor': { url: '/SellFactor/Add', type: 'post' },
    'edit-factor-item': { url: '/SellFactor/EditFactorItem', type: 'post' },
    'get-factor-Detail-by-id': { url: '/SellFactor/EditInitionalize', type: 'post' },
    'update-factor': { url: '/SellFactor/EditConfirm', type: 'post' },
    'get-site-value': { url: '/SiteValues/IndexReturnJson', type: 'post' },
    'get-today-date': { url: '/Home/GetDate', type: 'get' },
    'get-product-prices': { url: '/SellFactor/GetProductPrices', type: 'post' },
    'addon-code': { url: '/SellFactor/AddonCode', type: 'post' },
};


export const repository = {
    common: {

        getTodayDate: function () {
            var route = routes["get-today-date"];
            return serverResponseHandle(appConnection(route.url, route.type));
        }
    },
    factor: {
        getProduct: function (model) {
            var route = routes["get-product"];
            return serverResponseHandle(appConnection(route.url, route.type, model));
        },
        getCustomer: function (model) {
            var route = routes["get-search-Customer"];
            return serverResponseHandle(appConnection(route.url, route.type, model));
        },
        getCode: function (model) {
            var route = routes["get-code"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        initializeFactorRequirement: function (model) {
            var route = routes["initialize-requirement"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        addFactorItem: function (model) {
            var route = routes["add-factor-item"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        updateFactorItem: function (model) {
            var route = routes["edit-factor-item"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        addMainFactor: function (model) {
            var route = routes["add-main-factor"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        getSiteValue: function (model) {
            var route = routes["get-site-value"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        removeFactorItem: function (model) {
            var route = routes["remove-factor-item"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        getFactorDetailById: function (model) {
            var route = routes["get-factor-Detail-by-id"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        updateFactor: function (model) {
            var route = routes["update-factor"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        getProductPrices: function (model) {
            var route = routes["get-product-prices"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
        addonCode: function (model) {
            var route = routes["addon-code"];
            return serverResponseHandle(appConnection(route.url, route.type,model));
        },
    },


};


