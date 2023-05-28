
export function changeState(_this, param, val) {
    _this.setState({
        [param]: val
    });
    setTimeout(function () {
        $('.money').simpleMoneyFormat();
    }, 150);
}

export function checkNumericString(str) {

    if (str != undefined) {
        var res = "";
        res = str.toString().match(/[0-9]/i);
        return res;
    } else {
        return null;
    }
}