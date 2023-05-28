
import React from 'react';
import PropTypes from 'prop-types'
import ReactDOM from 'react-dom'
import { repository } from '../Server.js'
import { changeState, checkNumericString } from '../Common.js'
import { CrmSelect2 } from './crmReactSelectComponnent.js'

//import { NumeralInput } from 'react-numeral-input';
//let NumeralInput = require('react-numeral-input');
import {
    DatePicker,
    DateTimePicker,
    DateRangePicker,
    DateTimeRangePicker

} from "react-advance-jalaali-datepicker";
import { ToastContainer, toast } from 'react-toastify';
//import 'react-toastify/dist/ReactToastify.css';
class FactorForm extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            code: '',
            factorId: '',
            customer_id: '',
            dateTime: "",
            presentation: '',
            isRasmi: true,
            customerList: [],
            dropDownToSelect: false,
            customerSearch: '',
            getCustomerWaiting: false,
            getCodeWaiting: false,
            selectedCustomer: {},
            todayDate: "1396/05/15",
            placeOfDelivery: '',
            paymentDescription: '',
            person: '',
            expair: '',
            description: '',
            displayeDatePicker: false,
            addonCode: '',
            priceTotalItem: 0,
            factorRequirement: {},
            mainFactorVireModel: {},
            factorCostsInMainFactor: [],
            selectedItemToDelete: {},
            isConfirmValidationForm: false,
            factorDetailList: [],
            isEditMode: false,
            status: 5369
        }
        this.onchange = this.onchange.bind(this);
        this.openCustomerDropDown = this.openCustomerDropDown.bind(this);
        this.selectCustomer = this.selectCustomer.bind(this);
        this.setDate = this.setDate.bind(this);
        this.openAddProductModal = this.openAddProductModal.bind(this);
        this.closeCustomerDropDown = this.closeCustomerDropDown.bind(this);
        this.changeFactorCostInFactor = this.changeFactorCostInFactor.bind(this);
        this.removeFactorItem = this.removeFactorItem.bind(this);
        this.validationCode = this.validationCode.bind(this);
        this.addNewMainFactor = this.addNewMainFactor.bind(this);
        this.validationFormToSaveFactor = this.validationFormToSaveFactor.bind(this);
        this.openEditItemModal = this.openEditItemModal.bind(this);
        this.selectDynumicItem = this.selectDynumicItem.bind(this);
        this.updateSelectedMainFactor = this.updateSelectedMainFactor.bind(this);
        this.calculatinFactorItemPrice = this.calculatinFactorItemPrice.bind(this);
        this.addAddonFactor = this.addAddonFactor.bind(this);
        this.resetForm = this.resetForm.bind(this);
        this.onChangeFactorCostInItem = this.onChangeFactorCostInItem.bind(this);
        this.displayeDatePicker = this.displayeDatePicker.bind(this);
        window.factorFormComponent = this;
        toast.configure({
            autoClose: 2000,
            draggable: false,
            //etc you get the idea
        });
    }

    componentWillMount() {
        this.getTodayDate();
        this.getFactorRequirement();
        this.getSiteValue();

        setTimeout(function () {
            $('.money').simpleMoneyFormat();
        }, 200);
    }

    getFactorToEdit(query) {

        var _this = this;
        repository.factor.getFactorDetailById(query)
            .then(function (result) {

                var tempFactorItemList = [];

                var tempList = [];
                var tempList2 = [];
                var factorCosts = _this.state.factorRequirement.factorCosts;
                for (var i = 0; i < factorCosts.length; i++) {
                    if (factorCosts[i].isInFee === false && factorCosts[i].isInItem === true && factorCosts[i].isEnable === true) {
                        var temp = {};
                        for (var item in factorCosts[i]) {
                            temp[item] = factorCosts[i][item];
                        }
                        temp.value = 0;
                        tempList.push(temp);
                    } if (factorCosts[i].isInFee === true && factorCosts[i].isEnable === true) {
                        var temp2 = {};
                        for (var item in factorCosts[i]) {
                            temp2[item] = factorCosts[i][item];
                        }
                        temp2.value = 0;
                        tempList2.push(temp2);
                    }
                }



                for (var i = 0; i < result.data.data.FactorItemsNew.length; i++) {

                    var inItemFactorCosts = [];
                    var factorCostBeforFi = [];


                    for (var k = 0; k < tempList2.length; k++) {
                        var temp = {
                            id: tempList2[k].id,
                            isEnable: true,
                            isInCrease: tempList2[k].isInCrease,
                            isInFee: tempList2[k].isInFee,
                            isInItem: tempList2[k].isInItem,
                            isPresent: tempList2[k].isPresent,
                            isShowCustomer: tempList2[k].isShowCustomer,
                            name: tempList2[k].name,
                            value: 0
                        };

                        for (var j = 0; j < result.data.data.FactorItemsNew[i].factorCost.length; j++) {
                            var FactorCostSet_id = result.data.data.FactorItemsNew[i].factorCost[j].factorCostSet_id;
                            var id = tempList2[k].id;
                            if (result.data.data.FactorItemsNew[i].factorCost[j].factorCostSet_id === tempList2[k].id) {
                                temp.value = parseInt(result.data.data.FactorItemsNew[i].factorCost[j].value);
                            }
                        }
                        factorCostBeforFi.push(temp);
                    }

                    for (var l = 0; l < tempList.length; l++) {
                        var temp = {
                            id: tempList[l].id,
                            isEnable: true,
                            isInCrease: tempList[l].isInCrease,
                            isInFee: tempList[l].isInFee,
                            isInItem: tempList[l].isInItem,
                            isPresent: tempList[l].isPresent,
                            isShowCustomer: tempList[l].isShowCustomer,
                            name: tempList[l].name,
                            value: 0
                        };
                        for (var j = 0; j < result.data.data.FactorItemsNew[i].factorCost.length; j++) {
                            if (result.data.data.FactorItemsNew[i].factorCost[j].factorCostSet_id === tempList[l].id) {
                                temp.value = parseInt(result.data.data.FactorItemsNew[i].factorCost[j].value);
                            }
                        }
                        inItemFactorCosts.push(temp);
                    }


                    tempFactorItemList.push({
                        id: result.data.data.factorItem[i].id,
                        productId: result.data.data.factorItem[i].product_id,
                        factorId: result.data.data.factor.id,
                        priceId: result.data.data.factorItem[i].price_id,
                        priceVahed: result.data.data.factorItem[i].total / result.data.data.factorItem[i].count,
                        priceVahedToCosts: result.data.data.factorItem[i].total / result.data.data.factorItem[i].count,
                        price: result.data.data.factorItem[i].price,
                        count: result.data.data.factorItem[i].count,
                        priceTotalItem: result.data.data.factorItem[i].total,
                        garanty: result.data.data.factorItem[i].garanty,
                        warranty: result.data.data.factorItem[i].warranty,
                        description: result.data.data.factorItem[i].description,
                        FactorItemCosts: factorCostBeforFi,
                        inItemFactorCosts: inItemFactorCosts,
                        selectedProduct: result.data.data.factorItem[i].selectedProduct

                    });
                }
                _this.setState({
                    factorDetailList: tempFactorItemList,
                    code: result.data.data.factor.code,
                    dateTime: result.data.data.factor.dateTime,
                    //dateTime: "13970510",
                    selectedCustomer: { id: result.data.data.factor.customer_id, name: result.data.data.factor.customerName },
                    customer_id: result.data.data.factor.customer_id,
                    status: result.data.data.factor.statusId,
                    payment: result.data.data.factor.paymentTypeId,
                    person: result.data.data.factor.person === null ? "" : result.data.data.factor.person,
                    presentation: result.data.data.factor.presentationId,
                    isRasmi: result.data.data.factor.isRasmi,
                    expair: result.data.data.factor.expair === null ? 0 : result.data.data.factor.expair,
                    paymentDescription: result.data.data.factor.paymentDescription === null ? "" : result.data.data.factor.paymentDescription,
                    description: result.data.data.factor.description === null ? "" : result.data.data.factor.description,
                    factorId: result.data.data.factor.id,
                    placeOfDelivery: result.data.data.factor.deliveryPlace,
                    isEditMode: true

                });

                var costsInMainFactor = _this.state.factorCostsInMainFactor;
                for (var m = 0; m < costsInMainFactor.length; m++) {
                    for (var n = 0; n < result.data.data.factorCost.length; n++) {
                        if (costsInMainFactor[m].id === result.data.data.factorCost[n].id) {
                            costsInMainFactor[m].value = parseInt(result.data.data.factorCost[n].value);
                        }
                    }
                }
                changeState(_this, "factorCostsInMainFactor", costsInMainFactor);

                _this.functionHandel(_this, "summingUpAmounts");
                _this.functionHandel(_this, "finalPriceCalculating");
                setTimeout(function () {
                    _this.functionHandel(_this, "validationFormToSaveFactor");


                }, 500);

            });
    }

    getTodayDate() {
        var _this = this;
        repository.common.getTodayDate()
            .then(function (result) {
                changeState(_this, 'todayDate', result.data.data);

            });

    }

    addFactorItemToList(list) {
        changeState(this, "factorDetailList", list);
        this.functionHandel(this, "calculatinFactorItemPrice");

        this.functionHandel(this, "summingUpAmounts");
        this.functionHandel(this, "finalPriceCalculating");
        setTimeout(function () {
            $('.money').simpleMoneyFormat();
        }, 350);
    }

    functionHandel(context, fn) {
        if (context[fn]()) {
            setTimeout(function () {
                context[fn]();



            }, 200);
        }
        setTimeout(function () {
            $('.money').simpleMoneyFormat();
        }, 350);
    }

    getFactorRequirement() {
        var _this = this;

        changeState(this, "getCodeWaiting", true);


        repository.factor.initializeFactorRequirement()
            .then(function (result) {
                changeState(_this, "getCodeWaiting", false);

                if (result.data.statusCode === 200) {
                    changeState(_this, "code", result.data.data.code);
                    changeState(_this, "dateTime", result.data.data.dateTime.split(" ")[0]);
                    changeState(_this, "displayeDatePicker", true);

                    var tempList = [];
                    for (var i = 0; i < result.data.data.factorCosts.length; i++) {
                        result.data.data.factorCosts[i].value = 0;
                        if (result.data.data.factorCosts[i].isEnable === true && result.data.data.factorCosts[i].isInItem === false && result.data.data.factorCosts[i].isInFee === false) {
                            var temp = {};
                            for (var item in result.data.data.factorCosts[i]) {
                                temp[item] = result.data.data.factorCosts[i][item];
                            }
                            tempList.push(temp);
                        }
                    }
                    for (var j = 0; j < result.data.data.dynamics.length; j++) {
                        result.data.data.dynamics[j].value = 0;
                        for (var k = 0; k < result.data.data.siteVals.length; k++) {
                            if (result.data.data.dynamics[j].key == result.data.data.siteVals[k].Id) {
                                result.data.data.dynamics[j].name = result.data.data.siteVals[k].Name;
                            }
                        }
                    }
                    _this.setState({
                        factorRequirement: result.data.data,
                        factorCostsInMainFactor: tempList
                    });
                    if (window.location.search !== "" && window.location.search.indexOf("id=") > -1) {
                        _this.getFactorToEdit({ id: window.location.search.split("=")[1] });
                    }
                    if (window.location.search !== "" && window.location.search.indexOf("factorId=") > -1) {
                        changeState(_this, "isAddonView", true);
                        _this.getFactorToEdit({ id: window.location.search.split("=")[1] });
                        _this.addonCodeValidation(null);
                    }
                } else {
                    _this.callNotify("error", "بروز خطا در عملیات مقادیر فاکتور");
                }

            });

    }

    setStateFromout(item) {
        this.setState(item);
    }

    getCustomer() {

        var _this = this;
        changeState(this, "getCustomerWaiting", true);
        changeState(_this, "customerList", []);
        repository.factor.getCustomer({ value: this.state.customerSearch })
            .then(function (result) {
                if (result.data.statusCode === 200) {
                    changeState(_this, "customerList", result.data.data);
                } else {
                    _this.callNotify("error", "بروز خطا در عملیات جستجوی مشتری");

                }
                changeState(_this, "getCustomerWaiting", false);
            });

    }

    openCustomerDropDown(e) {
        e.stopPropagation();
        changeState(this, "dropDownToSelect", !this.state.dropDownToSelect);
    }

    stopPropagation(e) { e.stopPropagation(); }

    closeCustomerDropDown() {
        changeState(this, "dropDownToSelect", false);
    }

    selectCustomer(item) {
        this.setState({
            selectedCustomer: item,
            customer_id: item.id,
            dropDownToSelect: false
        });

        this.functionHandel(this, "validationFormToSaveFactor");
    }

    getSiteValue() {
        var _this = this;
        repository.factor.getSiteValue({ parentId: 7 })
            .then(function (result) {
                if (result.data.statusCode === 200) {
                    changeState(_this, "factorMessages", result.data.data);
                } else {
                    _this.callNotify("error", "بروز خطا در دریافت پیام های فاکتور");
                }
            });
    }

    onchange(e) {
        this.setState({
            [e.target.name]: e.target.value
        });
        if (e.target.name === "customerSearch") {
            var _this = this;
            changeState(this, "isChangeCustomerQuery", true);
            setTimeout(function () {
                changeState(_this, "isChangeCustomerQuery", false);
            }, 1000);
            setTimeout(function () {
                if (_this.state.isChangeProductQuery) {
                    return;
                }
                if (_this.state.isChangeCustomerQuery === false) {
                    _this.functionHandel(_this, "getCustomer");
                    changeState(_this, "isChangeCustomerQuery", undefined);
                }
            }, 2000);

        }
        this.functionHandel(this, "validationFormToSaveFactor");

    }

    getCode() {
        var _this = this;
        changeState(this, "getCodeWaiting", true);

        repository.factor.getCode()
            .then(function (result) {
                if (result.data.statusCode === 200) {
                    changeState(_this, "code", result.data.data);
                } else {
                    _this.callNotify("error", "بروز خطا در عملیات دریافت کد فاکتور");

                }
                changeState(_this, "getCodeWaiting", false);

            });

    }

    setDate(param1, param2) {
        changeState(this, 'dateTime', param2);
        this.functionHandel(this, "validationFormToSaveFactor");

    }

    validationCode() {
        var _this = this;
        changeState(this, "getCodeWaiting", true);
        repository.factor.getCode({ code: this.state.code })
            .then(function (result) {
                if (result.data.statusCode === 200) {
                    if (result.data.data === true) {
                    } else {
                        _this.callNotify("error", "بروز خطا در عملیات اعتبار سنجی  کد فاکتور");

                        changeState(_this, "code", '');

                    }

                }
                changeState(_this, "getCodeWaiting", false);

            });

    }

    openAddProductModal() {

        window.addProductComponent.openAddProductModal(this.state.factorRequirement.factorCosts);
    }

    callNotify(type, message) {
        toast[type](message, {
            position: "top-left",
            autoClose: 3000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true
        });

    }

    openDeleteItemConfirmModal(item) {

        changeState(this, "selectedItemToDelete", item);
        $("#removeFactoeDetailModal").modal("show");
    }

    openEditItemModal(item) {
        changeState(this, "selectedItemToEdit", item);

        window.addProductComponent.openEditModalDetail(item);
    }

    removeFactorItem() {
        var tempList = this.state.factorDetailList;
        var _this = this;
        repository.factor.removeFactorItem({ id: this.state.selectedItemToDelete.id })
            .then(function (result) {
                if (result.data.statusCode === 200) {
                    tempList.splice(tempList.indexOf(_this.state.selectedItemToDelete), 1);
                    changeState(_this, "factorDetailList", tempList);
                    $("#removeFactoeDetailModal").modal("hide");
                } else {
                    _this.callNotify("error", "بروز خطا در عملیات حذف آیتم فاکتور");
                }

            });



        this.functionHandel(this, "summingUpAmounts");
        this.functionHandel(this, "finalPriceCalculating");
    }

    onChangeFactorCostInItem(value, item, index, idx) {
        //var tempobj = this.state.factorRequirement;
        if (checkNumericString(value)) {
            var factorItemList = this.state.factorDetailList;
            factorItemList[idx].inItemFactorCosts[index].value = value === "" ? 0 : parseInt(value);
            this.functionHandel(this, "calculatinFactorItemPrice");
        }
    }

    calculatinFactorItemPrice() {
        var factorItemList = this.state.factorDetailList;


        for (var j = 0; j < factorItemList.length; j++) {
            factorItemList[j].priceVahed = parseInt(factorItemList[j].priceVahed);
            var priceVahedToCast = parseInt(factorItemList[j].priceVahed);

            for (var i = 0; i < factorItemList[j].inItemFactorCosts.length; i++) {
                if (factorItemList[j].inItemFactorCosts[i].isPresent) {
                    if (factorItemList[j].inItemFactorCosts[i].isInCrease) {
                        priceVahedToCast = Math.round(priceVahedToCast + (factorItemList[j].priceVahed * (factorItemList[j].inItemFactorCosts[i].value / 100)));

                    } else {
                        priceVahedToCast = Math.round(priceVahedToCast - (factorItemList[j].priceVahed * (factorItemList[j].inItemFactorCosts[i].value / 100)));

                    }
                } else {
                    if (factorItemList[j].inItemFactorCosts[i].isInCrease) {
                        priceVahedToCast = priceVahedToCast + parseInt(factorItemList[j].inItemFactorCosts[i].value);

                    } else {
                        priceVahedToCast = priceVahedToCast - parseInt(factorItemList[j].inItemFactorCosts[i].value);

                    }
                }
            }
            factorItemList[j].priceVahedToCosts = priceVahedToCast;

            factorItemList[j].priceTotalItem = factorItemList[j].priceVahedToCosts * factorItemList[j].count;


        }

        changeState(this, "factorDetailList", factorItemList);

        this.functionHandel(this, "summingUpAmounts");
        this.functionHandel(this, "finalPriceCalculating");

    }

    changeFactorCostInFactor(e, item, index) {
        if (checkNumericString(e.target.value)) {
            var _factorCostsInMainFactor = this.state.factorCostsInMainFactor;
            var val = e.target.value.replace(',', '');
            _factorCostsInMainFactor[index].value = e.target.value === "" ? 0 : parseInt(val);
            changeState(this, "factorCostsInMainFactor", _factorCostsInMainFactor);
            this.functionHandel(this, "finalPriceCalculating");
        }
    }

    summingUpAmounts() {
        var sum = 0;
        for (var i = 0; i < this.state.factorDetailList.length; i++) {
            sum += parseInt(this.state.factorDetailList[i].priceTotalItem);
        }
        changeState(this, "priceTotalItem", sum);
    }

    finalPriceCalculating() {

        var costList = this.state.factorCostsInMainFactor;
        var totalPrice = parseInt(this.state.priceTotalItem);
        var finalPrice = parseInt(this.state.priceTotalItem);
        for (var i = 0; i < costList.length; i++) {
            var val = parseInt(costList[i].value);

            if (costList[i].isInCrease) {
                if (costList[i].isPresent) {
                    finalPrice = Math.round(finalPrice + (totalPrice * (val / 100)));

                } else {
                    finalPrice = finalPrice + val;
                }
            } else {
                if (costList[i].isPresent) {
                    finalPrice = Math.round(finalPrice - (totalPrice * (val / 100)));

                } else {
                    finalPrice = finalPrice - val;

                }
            }
        }

        changeState(this, "priceTotalFactor", finalPrice);

    }

    validationFormToSaveFactor() {
        var _this = this;
        var addFactorCost = [];
        for (var i = 0; i < this.state.factorCostsInMainFactor.length; i++) {
            addFactorCost.push({
                factorId: this.state.factorId,
                factorCostId: this.state.factorCostsInMainFactor[i].id,
                value: this.state.factorCostsInMainFactor[i].value
            });
        }
        var factorDataModel = {
            factorId: this.state.factorId,
            code: this.state.code,
            customerId: this.state.customer_id,
            date: this.state.dateTime,
            presentationId: this.state.presentation,
            isRasmi: this.state.isRasmi,
            factorMessageId: 1,
            factorDescription: this.state.description,
            paymentDescription: this.state.paymentDescription,
            expireDate: this.state.expair,
            priceTotalFactor: this.state.priceTotalItem,
            priceFinish: this.state.priceTotalFactor,
            paymentTypeId: this.state.payment,
            deliveryPlace: this.state.placeOfDelivery,
            person: this.state.person,
            statusId: this.state.status,
            //addonCode: null,
            parentId: 7,
            addDynamics: [],
            addFactorCost: addFactorCost,
            factorCostAfter: addFactorCost
        };

        var result = true;
        for (var property in factorDataModel) {
            if (!factorDataModel[property]) {
                result = false;
            }
        }
        this.setState({
            isConfirmValidationForm: result,
            mainFactorVireModel: factorDataModel
        });
    }

    resetForm() {
        this.getFactorRequirement();
        this.getSiteValue();
        this.setState({
            code: '',
            factorId: '',
            customer_id: '',
            dateTime: "",
            presentation: '',
            isRasmi: true,
            customerList: [],
            dropDownToSelect: false,
            customerSearch: '',
            getCustomerWaiting: false,
            getCodeWaiting: false,
            selectedCustomer: {},
            todayDate: "1396/05/15",
            placeOfDelivery: '',
            paymentDescription: '',
            person: '',
            expair: '',
            description: '',
            addonCode: '',
            priceTotalItem: 0,
            factorRequirement: {},
            mainFactorVireModel: {},
            factorCostsInMainFactor: [],
            selectedItemToDelete: {},
            isConfirmValidationForm: false,
            factorDetailList: [],
            isEditMode: false,
            status: 5369
        });

        this.functionHandel(this, "getFactorRequirement");
        this.functionHandel(this, "getSiteValue");
    }

    addNewMainFactor() {

        var _this = this;
        changeState(this, "addMainFactorWaiting", true);
        var addFactorCost = [];
        for (var i = 0; i < this.state.factorCostsInMainFactor.length; i++) {
            addFactorCost.push({
                factorId: this.state.factorId,
                factorCostId: this.state.factorCostsInMainFactor[i].id,
                value: this.state.factorCostsInMainFactor[i].value
            });
        }

        var dynumicDataModel = [];

        for (var j = 0; j < this.state.factorRequirement.dynamics.length; j++) {
            dynumicDataModel.push({
                optionId: this.state.factorRequirement.dynamics[j].key,
                valueId: this.state.factorRequirement.dynamics[j].value,
                strValue: "",
                type: "DropDown",
            });
        }

        var factorCostAfters = [];
        for (var k = 0; k < this.state.factorDetailList.length; k++) {
            var ItemCostAfters = [];

            for (var l = 0; l < this.state.factorDetailList[k].inItemFactorCosts.length; l++) {
                ItemCostAfters.push({
                    id: this.state.factorDetailList[k].inItemFactorCosts[l].id,
                    factorItemId: this.state.factorDetailList[k].id,
                    costId: this.state.factorDetailList[k].inItemFactorCosts[l].id,
                    value: this.state.factorDetailList[k].inItemFactorCosts[l].value
                });
            }
            factorCostAfters.push({ ItemCostAfters: ItemCostAfters });
        }

        var factorDataModel = {
            factorId: this.state.factorId,
            code: this.state.code,
            customerId: this.state.customer_id,
            date: this.state.dateTime,
            presentationId: this.state.presentation,
            isRasmi: this.state.isRasmi,
            factorMessageId: null,
            factorDescription: this.state.description,
            paymentDescription: this.state.paymentDescription,
            expireDate: this.state.expair,
            priceTotalFactor: this.state.priceTotalItem,
            priceFinish: this.state.priceTotalFactor,
            paymentTypeId: this.state.payment,
            deliveryPlace: this.state.placeOfDelivery,
            person: this.state.person,
            statusId: this.state.status,
            addonCode: null,
            parentId: null,
            addDynamics: dynumicDataModel,
            addFactorCost: addFactorCost,
            factorCostAfters: factorCostAfters
        };

        repository.factor.addMainFactor(factorDataModel)
            .then(function (result) {
                changeState(_this, "addMainFactorWaiting", false);

                if (result.data.statusCode === 200) {
                    _this.callNotify("success", "عملیات با موفقیت انجام شد");
                    _this.resetForm();
                } else {
                    _this.callNotify("error", "بروز خطا در عملیات");


                }
            });


    }

    selectDynumicItem(e, item) {
        var tempDynumics = this.state.factorRequirement;
        tempDynumics.dynamics[parseInt(e.target.name)].value = e.target.value;
        changeState(this, "factorRequirement", tempDynumics);

    }

    updateSelectedMainFactor() {
        var _this = this;
        changeState(this, "addMainFactorWaiting", true);
        var addFactorCost = [];
        for (var i = 0; i < this.state.factorCostsInMainFactor.length; i++) {
            addFactorCost.push({
                factorId: this.state.factorId,
                factorCostId: this.state.factorCostsInMainFactor[i].id,
                value: this.state.factorCostsInMainFactor[i].value
            });
        }

        var dynumicDataModel = [];

        for (var j = 0; j < this.state.factorRequirement.dynamics.length; j++) {
            dynumicDataModel.push({
                optionId: this.state.factorRequirement.dynamics[j].key,
                valueId: this.state.factorRequirement.dynamics[j].value,
                strValue: "",
                type: "DropDown",
            });
        }

        var factorCostAfters = [];
        for (var k = 0; k < this.state.factorDetailList.length; k++) {
            var ItemCostAfters = [];

            for (var l = 0; l < this.state.factorDetailList[k].inItemFactorCosts.length; l++) {
                ItemCostAfters.push({
                    id: this.state.factorDetailList[k].inItemFactorCosts[l].id,
                    factorItemId: this.state.factorDetailList[k].id,
                    costId: this.state.factorDetailList[k].inItemFactorCosts[l].id,
                    value: this.state.factorDetailList[k].inItemFactorCosts[l].value
                });
            }
            factorCostAfters.push({ ItemCostAfters: ItemCostAfters });
        }

        var factorDataModelToUpdate = {
            factorId: this.state.factorId,
            code: this.state.code,
            customerId: this.state.customer_id,
            date: this.state.dateTime,
            presentationId: this.state.presentation,
            isRasmi: this.state.isRasmi,
            factorMessageId: null,
            factorDescription: this.state.description,
            paymentDescription: this.state.paymentDescription,
            expireDate: this.state.expair,
            priceTotalFactor: this.state.priceTotalItem,
            priceFinish: this.state.priceTotalFactor,
            paymentTypeId: this.state.payment,
            deliveryPlace: this.state.placeOfDelivery,
            person: this.state.person,
            statusId: this.state.status,
            addonCode: null,
            parentId: null,
            addDynamics: dynumicDataModel,
            addFactorCost: addFactorCost,
            factorCostAfters: factorCostAfters
        };

        repository.factor.updateFactor(factorDataModelToUpdate)
            .then(function (result) {
                changeState(_this, "addMainFactorWaiting", false);

                if (result.data.statusCode === 200) {
                    _this.callNotify("success", "عملیات با موفقیت انجام شد");
                    setTimeout(function () {
                        window.location = "/Employee/Factor";
                    },
                        4000);
                } else {
                    _this.callNotify("error", "بروز خطا در عملیات");


                }
            });

    }

    addAddonFactor() {
        var _this = this;
        changeState(this, "addMainFactorWaiting", true);
        var addFactorCost = [];
        for (var i = 0; i < this.state.factorCostsInMainFactor.length; i++) {
            addFactorCost.push({
                factorId: this.state.factorId,
                factorCostId: this.state.factorCostsInMainFactor[i].id,
                value: this.state.factorCostsInMainFactor[i].value
            });
        }

        var dynumicDataModel = [];

        for (var j = 0; j < this.state.factorRequirement.dynamics.length; j++) {
            dynumicDataModel.push({
                optionId: this.state.factorRequirement.dynamics[j].key,
                valueId: this.state.factorRequirement.dynamics[j].value,
                strValue: "",
                type: "DropDown",
            });
        }

        var factorCostAfters = [];
        for (var k = 0; k < this.state.factorDetailList.length; k++) {
            var ItemCostAfters = [];

            for (var l = 0; l < this.state.factorDetailList[k].inItemFactorCosts.length; l++) {
                ItemCostAfters.push({
                    id: this.state.factorDetailList[k].inItemFactorCosts[l].id,
                    factorItemId: this.state.factorDetailList[k].id,
                    costId: this.state.factorDetailList[k].inItemFactorCosts[l].id,
                    value: this.state.factorDetailList[k].inItemFactorCosts[l].value
                });
            }
            factorCostAfters.push({ ItemCostAfters: ItemCostAfters });
        }

        var factorDataModelToUpdate = {
            factorId: this.state.factorId,
            code: this.state.code,
            customerId: this.state.customer_id,
            date: this.state.dateTime,
            presentationId: this.state.presentation,
            isRasmi: this.state.isRasmi,
            factorMessageId: null,
            factorDescription: this.state.description,
            paymentDescription: this.state.paymentDescription,
            expireDate: this.state.expair,
            priceTotalFactor: this.state.priceTotalItem,
            priceFinish: this.state.priceTotalFactor,
            paymentTypeId: this.state.payment,
            deliveryPlace: this.state.placeOfDelivery,
            person: this.state.person,
            statusId: this.state.status,
            addonCode: null,
            parentId: null,
            addDynamics: dynumicDataModel,
            addFactorCost: addFactorCost,
            factorCostAfters: factorCostAfters
        };

        repository.factor.updateFactor({ model: factorDataModelToUpdate, parentId: this.state.factorId, addonCode: this.state.addonCode })
            .then(function (result) {
                changeState(_this, "addMainFactorWaiting", false);

                if (result.data.statusCode === 200) {
                    _this.callNotify("success", "عملیات با موفقیت انجام شد");
                    setTimeout(function () {
                        window.location = "/Employee/WorkFlow/FactorList";
                    },
                        3000);
                } else {
                    _this.callNotify("error", "بروز خطا در عملیات");


                }
            });

    }

    displayeDatePicker() {
        var param = this.state.displayDatePicker;
        param = !param;
        changeState(this, "displayDatePicker", param);
    }

    addonCodeValidation() {
        var _this = this;

        repository.factor.addonCode()
            .then(function (result) {
                if (result.data.statusCode === 200) {
                    changeState(_this, "addonCode", result.data.data);
                }
            });
    }

    render() {
        return (
            <div className={this.state.isAddonView === true ? 'addon-view' : ''} onClick={this.closeCustomerDropDown}>
                <div className="display-none page-title">

                    <div className="row">
                        <div className="col-lg-9 col-xs-12 ">
                            <h2>متمم فاکتور</h2>
                        </div>
                        <div className="col-lg-3 col-xs-12 ">


                        </div>
                    </div>



                </div>

                <ToastContainer
                    position="top-left"
                    autoClose={3000}
                    hideProgressBar={false}
                    newestOnTop={false}
                    closeOnClick
                    rtl={false}
                    pauseOnVisibilityChange
                    draggable
                    pauseOnHover
                />
                <div className="box-header with-border padding-20-wide">
                    <div className="padding-10-wide b gray-panel-shadow" >

                        {this.state.isAddonView !== true &&
                            <div className="row">
                                <div className="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                    <div className="form-group">
                                        <label>شماره</label>{this.state.code !== '' && <span className="valid-input"></span>}{this.state.code === '' && <span className="invalid-input"></span>}
                                        <input type="number" className="form-control" value={this.state.code} name={'code'} onChange={this.onchange} onBlur={this.validationCode} />

                                        {this.state.getCodeWaiting == true &&
                                            <span className="span-waiting-gray"  ></span>}
                                    </div>
                                </div>



                                <div className="col-lg-3 col-md-3 col-sm-6 col-xs-12" onClick={this.stopPropagation}>
                                    <div className="form-group">
                                        <label >مشتری</label>{this.state.selectedCustomer.name && <span className="valid-input"></span>}{!this.state.selectedCustomer.name && <span className="invalid-input"></span>}

                                        <CrmSelect2
                                            dropDownToSelect={this.state.dropDownToSelect}
                                            selectedCustomer={this.state.selectedCustomer}
                                            _onchange={this.onchange}
                                            customerList={this.state.customerList}
                                            _selectCustomer={this.selectCustomer}
                                            getCustomerWaiting={this.state.getCustomerWaiting}
                                            _openCustomerDropDown={this.openCustomerDropDown} />


                                        <span className="color-red" >*</span>
                                    </div>

                                </div>

                                <div className="col-lg-3 col-md-3 col-sm-6 col-xs-12">

                                    {this.state.isEditMode !== true &&
                                        <div>
                                            <label>تاریخ</label>{this.state.dateTime && <span className="valid-input"></span>}{!this.state.dateTime && <span className="invalid-input"></span>}
                                            {this.state.displayeDatePicker &&
                                                <div >
                                                    <DatePicker
                                                        placeholder="انتخاب تاریخ"
                                                        format="jYYYY/jMM/jDD"
                                                        id="dateTimePicker"

                                                        onChange={this.setDate}

                                                        placeholder="انتخاب تاریخ"

                                                        value={this.state.dateTime}
                                                        preSelected={this.state.dateTime}
                                                    />
                                                </div>
                                            }


                                        </div>
                                    }
                                    {this.state.isEditMode === true &&
                                        <div>
                                            <label>تاریخ</label>{this.state.dateTime && <span className="valid-input"></span>}{!this.state.dateTime && <span className="invalid-input"></span>}
                                            {this.state.displayDatePicker !== true &&
                                                <span className="span-input-mask">({this.state.dateTime})  <span className="spn-link-green" onClick={this.displayeDatePicker} >تغییر</span></span>
                                            }
                                            {this.state.displayDatePicker === true &&
                                                <div >
                                                    <DatePicker
                                                        placeholder="انتخاب تاریخ"
                                                        format="jYYYY/jMM/jDD"
                                                        id="dateTimePicker"

                                                        onChange={this.setDate}

                                                        placeholder="انتخاب تاریخ"

                                                        value={this.state.dateTime}
                                                        preSelected={this.state.dateTime}
                                                    />
                                                </div>
                                            }
                                        </div>
                                    }



                                </div>
                                <div className="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                                   
                                            <div className="form-group">
                                                <label>
                                                    رسمی
                                    <input type="radio" checked={this.state.isRasmi === true || this.state.isRasmi === "true"} name={'isRasmi'} value={true} onChange={this.onchange} />
                                                </label>
                                                <br />
                                                <label>
                                                    غیررسمی
                                    <input type="radio" checked={this.state.isRasmi === false ||
                                                        this.state.isRasmi === "false"} name={'isRasmi'} value={false} onChange={this
                                                            .onchange} />
                                                </label>

                                            </div>
                                      
                                </div>




                            </div>
                        }
                        {this.state.isAddonView === true &&
                            <div className="row">
                                <div className="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                    <div className="form-group">
                                        <label>شماره</label>{this.state.code && <span className="valid-input"></span>}{!this.state.code && <span className="invalid-input"></span>}
                                        <input type="number" className="form-control" value={this.state.code} onChange={this.onchange} onBlur={this.validationCode} />

                                        {this.state.getCodeWaiting == true &&
                                            <span className="span-waiting-gray"  ></span>}
                                    </div>
                                </div>

                                <div className="col-lg-2 col-md-2 col-sm-6 col-xs-12">

                                    <div className="form-group">
                                        <label>کد متمم</label>{this.state.addonCode && <span className="valid-input"></span>}{!this.state.addonCode && <span className="invalid-input"></span>}
                                        <input type="text" className="form-control" name={'addonCode'} onChange={this.onchange} value={this.state.addonCode} placeholder="کد متمم.." />

                                    </div>
                                </div>


                                <div className="col-lg-2 col-md-2 col-sm-6 col-xs-12" onClick={this.stopPropagation}>
                                    <div className="form-group">
                                        <label>مشتری</label>{this.state.selectedCustomer.name && <span className="valid-input"></span>}{!this.state.selectedCustomer.name && <span className="invalid-input"></span>}
                                        <CrmSelect2
                                            dropDownToSelect={this.state.dropDownToSelect}
                                            selectedCustomer={this.state.selectedCustomer}
                                            _onchange={this.onchange}
                                            customerList={this.state.customerList}
                                            _selectCustomer={this.selectCustomer}
                                            getCustomerWaiting={this.state.getCustomerWaiting}
                                            _openCustomerDropDown={this.openCustomerDropDown} />


                                    </div>

                                </div>

                                <div className="col-lg-2 col-md-2 col-sm-6 col-xs-12">


                                    <div>
                                        <label>تاریخ</label>{this.state.dateTime && <span className="valid-input"></span>}{!this.state.dateTime && <span className="invalid-input"></span>}
                                        {this.state.displayeDatePicker &&
                                            <div >
                                                <DatePicker
                                                    placeholder="انتخاب تاریخ"
                                                    format="jYYYY/jMM/jDD"
                                                    id="dateTimePicker"

                                                    onChange={this.setDate}

                                                    placeholder="انتخاب تاریخ"

                                                    value={this.state.dateTime}
                                                    preSelected={this.state.dateTime}
                                                />
                                            </div>
                                        }


                                    </div>




                                </div>
                                <div className="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                                   
                                      
                                            <div className="form-group">
                                                <label>
                                                    رسمی
                                    <input type="radio" checked={this.state.isRasmi === true || this.state.isRasmi === "true"} name={'isRasmi'} value={true} onChange={this.onchange} />
                                                </label>
                                                <br />
                                                <label>
                                                    غیررسمی
                                    <input type="radio" checked={this.state.isRasmi === false ||
                                                        this.state.isRasmi === "false"} name={'isRasmi'} value={false} onChange={this
                                                            .onchange} />
                                                </label>

                                            </div>
                                      
                                   

                                </div>




                            </div>
                        }


                    </div>
                </div>
                <div className=" padding-20 padding-to-botton-0">
                    <span title="اضافه کردن محصول" onClick={this.openAddProductModal} className="add-factor-detail-spn">
                        <svg viewBox="0 0 24 24">
                            <path d="M17,14H19V17H22V19H19V22H17V19H14V17H17V14M12,17V15H7V17H12M17,11H7V13H14.69C13.07,14.07 12,15.91 12,18C12,19.09 12.29,20.12 12.8,21H5C3.89,21 3,20.1 3,19V5C3,3.89 3.89,3 5,3H19A2,2 0 0,1 21,5V12.8C20.12,12.29 19.09,12 18,12L17,12.08V11M17,9V7H7V9H17Z" />
                        </svg></span>
                    {this.state.factorDetailList.length > 0 &&

                        <div className="padding-10-wide  gray-panel-shadow" >


                            <table className="table table-bordered tabel-white-header-black" >
                                <thead>
                                    <tr>
                                        <th>نام</th>
                                        <th>قیمت پایه</th>
                                        {this.state.factorRequirement.factorCosts && this.state.factorRequirement.factorCosts.map(item => item.isEnable === true && item.isInItem === true && item.isInFee === false &&
                                            <th key={item.id}>{item.name}

                                            </th>
                                        )}
                                        <th>قیمت واحد</th>
                                        <th>تعداد</th>
                                        <th>کل</th>
                                        <th>عملیات</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    {this.state.factorDetailList.map((item, idx) =>
                                        <tr key={item.id}>
                                            <td>{item.selectedProduct.name}</td>
                                        <td >{item.priceVahed}</td>
                                            {item.inItemFactorCosts.map((_item, index) =>
                                            <td className="dynamic-cousts-input" key={_item.id}>
                                                    <input type="text" 
                                                        name={'_item.name'}
                                                        value={_item.value}
                                                    className={_item.isPresent === false ? "form-control money" : "form-control"}
                                                    onChange={e => this.onChangeFactorCostInItem(e.target.value, _item, index, idx)} />
                                                    {_item.isPresent === true &&

                                                    <label className="percent-lab " className={_item.isInCrease === true ? "percent-lab isInCrease in-item" : "percent-lab un-isInCrease in-item"}>%</label>
                                                }
                                                    {_item.isPresent === false &&

                                                    <label className="number-lab " className={_item.isInCrease === true ? "percent-lab isInCrease in-item" : "percent-lab un-isInCrease in-item"}>ریال</label>



                                                }
                                                </td>
                                            )}
                                            <td>{item.priceVahedToCosts}</td>
                                            <td>{item.count}</td>
                                            <td>{item.priceTotalItem}</td>
                                            <td>
                                                <span className="delete-spn" onClick={() => this.openDeleteItemConfirmModal(item)}>
                                                    <svg viewBox="0 0 24 24">
                                                        <path d="M12,20C7.59,20 4,16.41 4,12C4,7.59 7.59,4 12,4C16.41,4 20,7.59 20,12C20,16.41 16.41,20 12,20M12,2C6.47,2 2,6.47 2,12C2,17.53 6.47,22 12,22C17.53,22 22,17.53 22,12C22,6.47 17.53,2 12,2M14.59,8L12,10.59L9.41,8L8,9.41L10.59,12L8,14.59L9.41,16L12,13.41L14.59,16L16,14.59L13.41,12L16,9.41L14.59,8Z" />
                                                    </svg>
                                                </span>
                                                <span className="edit-spn" onClick={() => this.openEditItemModal(item)}>
                                                    <svg viewBox="0 0 24 24">
                                                        <path d="M8,12H16V14H8V12M10,20H6V4H13V9H18V12.1L20,10.1V8L14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H10V20M8,18H12.1L13,17.1V16H8V18M20.2,13C20.3,13 20.5,13.1 20.6,13.2L21.9,14.5C22.1,14.7 22.1,15.1 21.9,15.3L20.9,16.3L18.8,14.2L19.8,13.2C19.9,13.1 20,13 20.2,13M20.2,16.9L14.1,23H12V20.9L18.1,14.8L20.2,16.9Z" />
                                                    </svg>
                                                </span>
                                            </td>
                                        </tr>
                                    )}

                                </tbody>
                            </table>



                        </div>}
                </div>
                {this.state.factorDetailList.length > 0 &&
                    <div className="main-factor-detail padding-20">
                        <div className="gray-panel-shadow border-lightstlb-2 padding-20 no-margin-top" >
                            <h4>مقادیر فاکتور</h4>
                            <div className="row">
                                <div className="col-lg-3 col-md-3 col-xs-12 col-sm-6">
                                    <div className="form-group  form-group-light">
                                        <label>مجموع</label>
                                        <input type="text" className="form-control money" onChange={this.onchange} value={this.state.priceTotalItem} name={'priceTotalItem_'} />
                                    </div>
                                </div>
                                {this.state.factorCostsInMainFactor && this.state.factorCostsInMainFactor.map((item, index) =>
                                    <div className="col-lg-3 col-md-3 col-xs-12 col-sm-6" key={item.id}>

                                        <div className="form-group  form-group-light dynamic-cousts-input">
                                            <label >{item.name}

                                              
                                             
                                            </label>
                                            <input type="text" className={item.isPresent === false ? "form-control money" : "form-control"} onChange={(e, item) => this.changeFactorCostInFactor(e, item, index)} value={item.value} name={item.name} />
                                            {item.isPresent === true &&

                                                <label className="percent-lab" className={item.isInCrease === true ? "percent-lab isInCrease" : "percent-lab un-isInCrease"}>%</label>
                                            }
                                            {item.isPresent === false &&

                                                <label className="number-lab" className={item.isInCrease === true ? "percent-lab isInCrease" : "percent-lab un-isInCrease"}>ریال</label>



                                            }
                                        </div>
                                    </div>
                                )}
                                <div className="col-lg-3 col-md-3 col-xs-12 col-sm-6">
                                    <div className="form-group  form-group-light">
                                        <label>مبلغ نهایی</label>
                                        <input type="text" className="form-control money" defaultValue={0} onChange={this.onchange} value={this.state.priceTotalFactor} name={'priceTotalFactor_'} />
                                    </div>
                                </div>
                              



                            </div>

                        </div>
                       
                            <div className="gray-panel-shadow border-lightstlb-2 padding-20" >
                                <h4>داینامیک فاکتور</h4>
                                <div className="row">
                                    {this.state.factorRequirement.dynamics.map((item, index) =>
                                        <div className="col-lg-3 col-md-3 col-xs-12 col-sm-6" key={item.key}>
                                            <div className="form-group  form-group-light">
                                                <label>{item.name}</label>
                                                <select className="form-control" name={index} onChange={(e) => this.selectDynumicItem(e, item)}>
                                                    <option></option>
                                                    {item.values.map(option =>
                                                        <option value={option.id} key={option.id}>{item.vahedName} {option.name}</option>
                                                    )}
                                                </select>
                                            </div>
                                        </div>
                                    )}




                                </div>

                            </div>

                        <div className="gray-panel-shadow border-lightstlb-2 padding-20" >
                            <h4>اطلاعات فاکتور</h4>
                            <div className="row">
                                <div className="col-lg-3 col-md-3 col-xs-12 col-sm-6">
                                    <div className="form-group  form-group-light">
                                        <label>اعتبار قیمت</label>{this.state.expair && <span className="valid-input"></span>}{!this.state.expair && <span className="invalid-input"></span>}
                                        <input type="text" className="form-control" placeholder="روز.." onChange={this.onchange} value={this.state.expair} name={'expair'} />
                                    </div>
                                </div>
                                <div className="col-lg-3 col-md-3 col-xs-12 col-sm-6">

                                    <div className="form-group  form-group-light">
                                        <label>قابل توجه</label>{this.state.person && <span className="valid-input"></span>}{!this.state.person && <span className="invalid-input"></span>}
                                        <input type="text" className="form-control" onChange={this.onchange} value={this.state.person} name={'person'} />
                                    </div>
                                </div>
                                <div className="col-lg-3 col-md-3 col-xs-12 col-sm-6">

                                    <div className="form-group  form-group-light">
                                        <label>نحوه پرداخت</label>{this.state.payment && <span className="valid-input"></span>}{!this.state.payment && <span className="invalid-input"></span>}
                                        <select className="form-control" value={this.state.payment} onChange={this.onchange} name={'payment'}>
                                            <option value={undefined}></option>
                                            <option value="5386">قسطی</option>
                                            <option value="5387">چکی</option>
                                            <option value="5406">نقدی</option>
                                        </select>
                                    </div>
                                </div>
                                <div className="col-lg-3 col-md-3 col-xs-12 col-sm-6">

                                    <div className="form-group  form-group-light">
                                        <label>محل تحویل</label>{this.state.placeOfDelivery && <span className="valid-input"></span>}{!this.state.placeOfDelivery && <span className="invalid-input"></span>}
                                        <input type="text" className="form-control" onChange={this.onchange} value={this.state.placeOfDelivery} name={'placeOfDelivery'} />
                                    </div>
                                </div>


                                <hr />
                                <div className="col-lg-4 col-md-4 col-xs-12 col-sm-6">

                                    <div className="form-group  form-group-light">
                                        <label>توضیحات فاکتور</label>{this.state.description && <span className="valid-input"></span>}{!this.state.description && <span className="invalid-input"></span>}
                                        <textarea className="form-control" value={this.state.description} onChange={this.onchange} name={'description'}>{this.state.description}</textarea>
                                    </div>
                                </div>


                                <div className="col-lg-4 col-md-4 col-xs-12 col-sm-6">

                                    <div className="form-group  form-group-light">
                                        <label>توضیحات پرداخت</label>{this.state.paymentDescription && <span className="valid-input"></span>}{!this.state.paymentDescription && <span className="invalid-input"></span>}
                                        <textarea className="form-control" value={this.state.paymentDescription} name={'paymentDescription'} onChange={this.onchange}>{this.state.paymentDescription}</textarea>
                                    </div>
                                </div>


                                <div className="col-lg-4 col-md-4 col-xs-12 col-sm-6">

                                    <div className="form-group  form-group-light">
                                        <label>(اختیاری) پیام فاکتور</label><span className="color-red">در آینده برای چاپ استفاده می شود</span>
                                        <textarea className="form-control" ></textarea>
                                    </div>
                                </div>



                            </div>

                        </div>
                    </div>
                }
                <div className="row">
                    {this.state.factorDetailList.length > 0 && this.state.isEditMode !== true && this.state.isAddonView !== true &&
                        <div className=" col-lg-1 col-xs-12 padding-20">
                            <input type="button" className="btn btn-block btn-success" disabled={!this.state.isConfirmValidationForm || this.state.addMainFactorWaiting === true} value="ثبت" onClick={this.addNewMainFactor} />
                            {this.state.addMainFactorWaiting === true &&
                                <span className="span-waiting"  ></span>}
                        </div>
                    }
                    {this.state.factorDetailList.length > 0 && this.state.isEditMode === true && this.state.isAddonView !== true &&
                        <div className=" col-lg-1 col-xs-12 padding-20">
                            <input type="button" className="btn btn-block btn-primary" disabled={!this.state.isConfirmValidationForm || this.state.addMainFactorWaiting === true} value="بروز رسانی" onClick={this.updateSelectedMainFactor} />
                            {this.state.addMainFactorWaiting === true &&
                                <span className="span-waiting"  ></span>}
                        </div>
                    }
                    {this.state.factorDetailList.length > 0 && this.state.isEditMode === true && this.state.isAddonView === true &&
                        <div className=" col-lg-1 col-xs-12 padding-20">
                            <input type="button" className="btn btn-block btn-default" disabled={!this.state.isConfirmValidationForm || this.state.addMainFactorWaiting === true || this.state.addonCode === ''} value="ثبت متمم" onClick={this.addAddonFactor} />
                            {this.state.addMainFactorWaiting === true &&
                                <span className="span-waiting"  ></span>}
                        </div>
                    }

                </div>


            </div>
        );
    }
}

class FactorDetail extends React.Component {
    constructor() {
        super();
        this.state = {
            dropDownToSelect: false,
            productList: [],
            selectedProduct: {},
            getProductWaiting: false,
            drpMessage: "لطفا 1 یا چند کارکتر وارد کنید",
            productSearch: '',
            factorCosts: [],
            productId: 0,
            factorId: 0,
            factorItemId: 0,
            priceId: 0,
            price: 0,
            count: 1,
            priceTotalItem: 0,
            garanty: 0,
            priceVahed: 0,
            priceVahedName: '',
            warranty: 0,
            description: '',
            InitializedFactorCosts: [],
            inItemFactorCosts: [],
            isEditModal: false

        }
        this.closeModal = this.closeModal.bind(this);
        this.selectProduct = this.selectProduct.bind(this);
        this.openProductDropDown = this.openProductDropDown.bind(this);
        this.getProduct = this.getProduct.bind(this);
        this.onchange = this.onchange.bind(this);
        this.onChangeFactorCostItem = this.onChangeFactorCostItem.bind(this);
        this.addFactorItem = this.addFactorItem.bind(this);
        this.selectPrice = this.selectPrice.bind(this);
        this.updateFactorItem = this.updateFactorItem.bind(this);
        this.closeCustomerDropDown = this.closeCustomerDropDown.bind(this);
        window.addProductComponent = this;

    }

    componentWillMount() {
        changeState(this, "dropDownToSelect", false);
        setTimeout(function () {
            $('.money').simpleMoneyFormat();

        }, 2000);
    }

    selectProduct(item) {
        this.setState({
            productId: item.id,
            selectedProduct: item,
            dropDownToSelect: false
        });

    }

    closeModal() {
        $("#addFactoeDetailModal").modal("hide");

    }

    openAddProductModal(factorCosts) {


        this.setState({
            isEditModal: false,
            productId: 0,
            selectedProduct: {},
            dropDownToSelect: false,
            priceId: 0,
            priceVahed: 0,
            price: 0,
            count: 1,
            priceTotalItem: 0,
            garanty: 0,
            description: '',
            InitializedFactorCosts: factorCosts,
            factorCosts: factorCosts

        });



        var tempList = [];
        var tempList2 = [];
        for (var i = 0; i < factorCosts.length; i++) {
            if (factorCosts[i].isInFee === false && factorCosts[i].isInItem === true && factorCosts[i].isEnable === true) {
                var temp = {};
                for (var item in factorCosts[i]) {
                    temp[item] = factorCosts[i][item];
                }
                temp.value = 0;
                tempList.push(temp);
            } if (factorCosts[i].isInFee === true && factorCosts[i].isEnable === true) {
                var temp2 = {};
                for (var item in factorCosts[i]) {
                    temp2[item] = factorCosts[i][item];
                }
                temp2.value = 0;
                tempList2.push(temp2);
            }
        }
        changeState(this, "inItemFactorCosts", tempList);
        changeState(this, "InitializedFactorCosts", tempList2);

        $("#addFactoeDetailModal").modal("show");

    }

    openProductDropDown(e) {
        e.stopPropagation();
        changeState(this, "dropDownToSelect", !this.state.dropDownToSelect);
    }

    stopPropagation(e) { e.stopPropagation(); }

    closeCustomerDropDown() {
        changeState(this, "dropDownToSelect", false);
        setTimeout(function () {
            $('.money').simpleMoneyFormat();
        }, 350);
    }

    onchange(e) {
        var _this = this;
        this.setState({
            [e.target.name]: e.target.value
        });

        if (e.target.name === "productSearch") {
            changeState(this, "isChangeProductQuery", true);

            setTimeout(function () {
                changeState(_this, "isChangeProductQuery", false);


            }, 1000);

            setTimeout(function () {

                if (_this.state.isChangeProductQuery) {
                    return;
                }
                if (_this.state.isChangeProductQuery === false) {

                    _this.functionHandel(_this, "getProduct");
                    changeState(_this, "isChangeProductQuery", undefined);

                }

            }, 2000);
        }
        if (e.target.name === "count") {
            var val = e.target.value === "" ? 1 : e.target.value;
            changeState(this, "count", parseInt(val));
            this.functionHandel(this, "calculateProductItemPrice");
        }
        if (e.target.name === "price") {
            var val = e.target.value === "" ? 0 : e.target.value;
            changeState(this, "price", val);
            this.functionHandel(this, "calculateProductItemPrice");
        }

    }

    functionHandel(context, fn) {
        setTimeout(function () { context[fn](); }, 200);
        setTimeout(function () {
            $('.money').simpleMoneyFormat();
        }, 350);
    }

    selectPrice(e) {

        var temp = this.state.selectedProduct.prices;
        for (var i = 0; i < temp.length; i++) {
            if (temp[i].id === parseInt(e.target.value)) {
                this.setState({
                    count: 1,
                    price: parseInt(temp[i].price),
                    priceId: temp[i].id,
                    priceTotalItem: parseInt(temp[i].price),
                    priceVahed: parseInt(temp[i].price),
                    priceVahedName: temp[i].vahedName
                });
            }
        }
        var _tempList = this.state.factorCosts;
        for (var i = 0; i < _tempList.length; i++) {
            _tempList[i].value = 0;
        }
        changeState(this, "factorCosts", _tempList);
        setTimeout(function () {
            $('.money').simpleMoneyFormat();

        }, 500);
    }

    getProduct() {
        var _this = this;
        changeState(this, "getProductWaiting", true);
        repository.factor.getProduct({ value: this.state.productSearch })
            .then(function (result) {
                if (result.data.statusCode === 200) {
                    changeState(_this, "productList", result.data.data);
                } else {
                    //window.factorFormComponent.callNotify("error", "بروز خطا در عملیات جستجوی کالا");

                }
                changeState(_this, "getProductWaiting", false);
            });


    }

    onChangeFactorCostItem(value, item, index) {
        if (checkNumericString(value)) {


            var temp = this.state.factorCosts;
            var val = value.replace(',', '');

            temp[index].value = value === "" ? 0 : parseInt(val);
            changeState(this, "factorCosts", temp);

            this.functionHandel(this, "calculateProductItemPrice");
        }
    }

    calculateProductItemPrice() {
        var tempList = [];
        for (var i = 0; i < this.state.factorCosts.length; i++) {
            if (this.state.factorCosts[i].isInFee === true && this.state.factorCosts[i].isEnable) {
                tempList.push(this.state.factorCosts[i]);

            }
        }
        var costs = parseInt(this.state.price);
        changeState(this, "InitializedFactorCosts", tempList);

        for (var j = 0; j < tempList.length; j++) {
            if (tempList[j].isInCrease) {
                if (tempList[j].isPresent) {
                    var perCost = this.state.price * (parseInt(tempList[j].value) / 100);
                    costs = costs + perCost;

                } else {
                    costs = costs + parseInt(tempList[j].value);
                }


            } else {
                if (tempList[j].isPresent) {
                    var perCost = this.state.price * (parseInt(tempList[j].value) / 100);
                    costs = costs - perCost;

                } else {
                    costs = costs - parseInt(tempList[j].value);

                }

            }
        }
        const totalPrice = costs * parseInt(this.state.count);
        changeState(this, "priceVahed", costs);
        changeState(this, "priceTotalItem", totalPrice);
        setTimeout(function () {
            $('.money').simpleMoneyFormat();

        }, 500);
    }

    addFactorItem() {
        const _this = this;
        changeState(this, "waiting", true);
        const dataModel = {
            id: this.state.factorId === 0 ? null : this.state.factorId, model: {
                productId: this.state.productId,
                factorId: this.state.factorId,
                priceId: this.state.priceId,
                priceVahed: this.state.priceVahed,
                priceVahedToCosts: this.state.priceVahed,
                price: this.state.price,
                count: this.state.count,
                priceTotalItem: this.state.priceTotalItem,
                garanty: this.state.garanty,
                warranty: this.state.warranty,
                description: this.state.description,
                FactorItemCosts: this.state.InitializedFactorCosts

            }
        }


        repository.factor.addFactorItem(dataModel)
            .then(function (result) {
                changeState(_this, "waiting", false);

                var tempList = window.factorFormComponent.state.factorDetailList;
                dataModel.model.factorId = 1;
                dataModel.model.selectedProduct = _this.state.selectedProduct;
                dataModel.model.inItemFactorCosts = _this.state.inItemFactorCosts;
                dataModel.model.priceVahedToCosts = dataModel.model.priceVahed;
                if (result.data.statusCode === 200) {

                    dataModel.model.factorId = result.data.data.factorId;
                    dataModel.model.id = result.data.data.factorItemId;

                    changeState(_this, "factorId", result.data.data.factorId);

                    window.factorFormComponent.setStateFromout({ factorId: result.data.data.factorId });
                    tempList.push(dataModel.model);
                    window.factorFormComponent.addFactorItemToList(tempList);



                    $("#addFactoeDetailModal").modal("hide");
                    window.factorFormComponent.callNotify("success", "عملیات با موفقیت انجام شد");
                    var _tempList = [];
                    for (var i = 0; i < _this.state.factorCosts.length; i++) {
                        _tempList.push({
                            id: _this.state.factorCosts[i].id,
                            isEnable: _this.state.factorCosts[i].isEnable,
                            isInCrease: _this.state.factorCosts[i].isInCrease,
                            isInFee: _this.state.factorCosts[i].isInFee,
                            isInItem: _this.state.factorCosts[i].isInItem,
                            isPresent: _this.state.factorCosts[i].isPresent,
                            isShowCustomer: _this.state.factorCosts[i].isShowCustomer,
                            name: _this.state.factorCosts[i].name,
                            value: 0
                        });
                    }

                    _this.setState({
                        productId: 0,
                        selectedProduct: {},
                        dropDownToSelect: false,
                        priceId: 0,
                        priceVahed: 0,
                        price: 0,
                        count: 1,
                        priceTotalItem: 0,
                        garanty: 0,
                        description: '',
                        InitializedFactorCosts: _tempList,
                        factorCosts: _tempList

                    });

                } else {
                    window.factorFormComponent.callNotify("error", "بروز خطا در عملیات اضافه کردن محصول به فاکتور");

                }


            });
    }

    openEditModalDetail(item) {

        var _this = this;
        this.setState({
            productId: item.selectedProduct.id,
            selectedProduct: item.selectedProduct,
            dropDownToSelect: false,
            priceId: item.priceId,
            priceVahed: item.priceVahed,
            price: item.price,
            count: item.count,
            priceTotalItem: item.priceTotalItem,
            garanty: item.garanty,
            description: item.description,
            InitializedFactorCosts: item.FactorItemCosts,
            factorCosts: item.FactorItemCosts,
            isEditModal: true,
            selectedFactorItemToEdit: item

        });


        repository.factor.getProductPrices({ id: item.selectedProduct.id })
            .then(function (result) {

                if (result.data.statusCode === 200) {
                    item.selectedProduct.code = 0;
                    item.selectedProduct.prices = result.data.data;
                    changeState(_this, "selectedProduct", item.selectedProduct);


                    for (var i = 0; i < result.data.data.length; i++) {
                        if (result.data.data[i].price === parseInt(item.price)) {
                            _this.setState({
                                priceId: result.data.data[i].id
                            });
                        }
                    }


                }
            });
        $("#addFactoeDetailModal").modal("show");
    }

    updateFactorItem() {
        var originList = window.factorFormComponent.state.factorDetailList;
        var idx = originList.indexOf(this.state.selectedFactorItemToEdit);

        var _this = this;
        changeState(this, "waiting", true);
        const dataModel = {
            id: this.state.selectedFactorItemToEdit.id,
            factorItemId: this.state.selectedFactorItemToEdit.id,
            selectedProduct: this.state.selectedProduct,
            productId: this.state.productId,
            factorId: this.state.factorId,
            priceId: this.state.priceId,
            priceVahed: this.state.priceVahed,
            price: this.state.price,
            count: this.state.count,
            priceTotalItem: this.state.priceTotalItem,
            garanty: this.state.garanty,
            warranty: this.state.warranty,
            description: this.state.description,
            FactorItemCosts: this.state.InitializedFactorCosts,
            inItemFactorCosts: this.state.selectedFactorItemToEdit.inItemFactorCosts,
            //priceVahedToCosts: this.state.priceVahed

        }

        repository.factor.updateFactorItem(dataModel)
            .then(function (result) {
                if (result.data.statusCode === 200) {

                    originList[idx] = dataModel;

                    window.factorFormComponent.addFactorItemToList(originList);
                    window.factorFormComponent.callNotify("success", "عملیت با موفقیت انجام شد");
                    $("#addFactoeDetailModal").modal("hide");

                } else {
                    window.factorFormComponent.callNotify("error", "بروز خطا در عملیات بروزرسانی آیتم فاکتور");

                }
                changeState(_this, "waiting", false);

            });
    }

    render() {
        return (
            <div className="modal-dialog modal-lg add-product-to-factor-modal">
                <div className="modal-content"  >

                    <div className="modal-header bg-primary">
                        <div className="row">
                            <div className="col-lg-8 col-md-8 col-sm-8">
                                <h5 >اضافه کردن محصول</h5>
                            </div>
                            <div className="col-lg-4 col-md-4 col-sm-4">
                                <button type="button" className="close" onClick={this.closeModal} aria-label="Close">
                                    <em className="ion-ios-close-empty sn-link-close"></em>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div className="modal-body" onClick={this.closeCustomerDropDown}>
                        <div className="row">
                            <div className="col-lg-7 col-md-7 col-xs-12">
                                <div className="row">
                                    <div className="col-lg-6 col-md-6 col-xs-12">
                                        <div className="form-group" onClick={this.stopPropagation}>
                                            <label>انتخاب کالا</label>
                                            <div className="crm-react-select2" className={this.state.dropDownToSelect === true ? "crm-react-select2 open" : "crm-react-select2"}>
                                                <label className="selected-item-name" onClick={this.openProductDropDown} >

                                                    <span className="selected-name-val" >
                                                        {this.state.selectedProduct.name}
                                                    </span>
                                                    <span className="per">
                                                        {this.state.dropDownToSelect === false &&
                                                            <svg viewBox="0 0 24 24">
                                                                <path fill="#000000" d="M7.41,8.58L12,13.17L16.59,8.58L18,10L12,16L6,10L7.41,8.58Z" />
                                                            </svg>
                                                        }
                                                        {this.state.dropDownToSelect === true &&
                                                            <svg viewBox="0 0 24 24">
                                                                <path fill="#000000" d="M7.41,15.41L12,10.83L16.59,15.41L18,14L12,8L6,14L7.41,15.41Z" />
                                                            </svg>
                                                        }
                                                    </span>
                                                </label>

                                                <div className="tool-box" >
                                                    <div className="search-input-container" >
                                                        <input type="text" className="form-control" name={'productSearch'} onChange={this.onchange} /></div>
                                                    {this.state.getProductWaiting == false && this.state.productList.length == 0 && <span className="drop-down-message" > {this.state.drpMessage} </span>}
                                                    {this.state.getProductWaiting == true && <span className="span-waiting"  ></span>}
                                                    <ul>
                                                        {this.state.productList.map(item =>
                                                            <li key={item.id} onClick={() => this.selectProduct(item)} >{item.name}</li>)}

                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div className="col-lg-6 col-md-6 col-xs-12">
                                        <div className="form-group">
                                            <label>واحد پول</label>
                                            <select className="form-control" value={this.state.priceId} onChange={(e) => this.selectPrice(e)}>
                                                <option></option>
                                                {this.state.selectedProduct.prices && this.state.selectedProduct.prices.map(item =>
                                                    <option value={item.id} key={item.id}>{item.vahedName} {item.price}</option>
                                                )}
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col-lg-5 col-md-5 col-xs-12">
                                <div className="form-group dynamic-cousts-input">
                                    <label>فی پایه</label>
                                    <input type="text" className="form-control money" placeholder="قیمت پایه.." value={this.state.price} name={'price'} onChange={this.onchange} />
                                    <label className="month-lab" >ریال</label>
                                </div>
                            </div>







                            <div className="col-lg-12 col-md-12 col-xs-12 col-sm-12">
                                <span className="line"></span>

                                <div className="row">
                                    {this.state.factorCosts.map((item, index) => item.isInFee === true && item.isEnable === true &&

                                        <div key={item.id} className="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                            <div className="form-group  dynamic-cousts-input">
                                                <label>{item.name}
                                                    {item.isInCrease === true && <label className="margin-right-5">افزایشی</label>
                                                    }
                                                    {item.isInCrease === false && <label className="margin-right-5">کاهشی</label>
                                                    }


                                                </label>
                                                <input type="text" className="form-control " className={item.isPresent === false ? "form-control money" : "form-control"} value={item.value} onChange={e => this.onChangeFactorCostItem(e.target.value, item, index)} />
                                                {item.isPresent === true &&

                                                    <label className="percent-lab" className={item.isInCrease === true ? "percent-lab isInCrease" : "percent-lab un-isInCrease"}>%</label>
                                                }
                                                {item.isPresent === false &&

                                                    <label className="number-lab" className={item.isInCrease === true ? "percent-lab isInCrease" : "percent-lab un-isInCrease"}>ریال</label>



                                                }
                                            </div>
                                        </div>)}
                                </div>




                            </div>


                            <div className="col-lg-12 col-md-12 col-xs-12">
                                <span className="line"></span>

                                <div className="row">
                                    <div className="col-lg-6 col-md-6 col-xs-12">
                                        <div className="form-group ">
                                            <label>قیمت واحد</label>
                                            <input type="text" className="form-control money" value={this.state.priceVahed} name={'priceVahed_'} onChange={this.onchange} placeholder="قیمت واحد.." />
                                        </div>
                                    </div>
                                    <div className="col-lg-6 col-md-6 col-xs-12">
                                        <div className="form-group ">
                                            <label>تعداد</label>
                                            <input type="text" className="form-control " value={this.state.count} name={'count'} onChange={this.onchange} placeholder="تعداد.." />
                                        </div>
                                    </div>
                                    <div className="col-lg-6 col-md-6 col-xs-12">
                                        <div className="form-group ">
                                            <label>قیمت کل</label>
                                            <input type="text" className="form-control money" value={this.state.priceTotalItem} name={'priceTotalItem_'} onChange={this.onchange} placeholder="قیمت کل.." />
                                        </div>
                                    </div>
                                    <div className="col-lg-6 col-md-6 col-xs-12">
                                        <div className="form-group dynamic-cousts-input">
                                            <label>گارانتی</label>
                                            <input type="number" name={'garanty'} onChange={this.onchange} value={this.state.garanty} className="form-control" placeholder="ماه" />
                                            <label className="month-lab" >ماه</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="modal-footer">
                        {!this.state.isEditModal &&
                            <div className="col-lg-2" >
                                <input type="button" className="btn btn-block btn-success " onClick={this.addFactorItem} disabled={!this.state.priceVahed || this.state.waiting} value="اضافه" />
                                {this.state.waiting &&
                                    <span className="span-waiting"></span>
                                }
                            </div>}
                        {this.state.isEditModal &&

                            <div className="col-lg-2" >
                                <input type="button" className="btn btn-block btn-primary " onClick={this.updateFactorItem} disabled={!this.state.priceVahed || this.state.waiting} value="بروزرسانی" />
                                {this.state.waiting &&
                                    <span className="span-waiting"></span>
                                }
                            </div>
                        }


                    </div>

                </div>
            </div>
        )
    }

}

class FactorItemDelete extends React.Component {

    closeModa() {

    }
    deleteItem() {

        window.factorFormComponent.removeFactorItem();
    }

    render() {
        return (
            <div className="modal-dialog modal-md">
                <div className="modal-content " >
                    <div className="modal-header ">
                        <div className="row">
                            <div className="col-lg-8 col-md-8 col-sm-8">
                                <h5 ></h5>
                            </div>
                            <div className="col-lg-4 col-md-4 col-sm-4">
                                <button type="button" className="close" onClick={this.closeModa} aria-label="Close">
                                    <em className="ion-ios-close-empty sn-link-close"></em>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div className="modal-body white-modal">
                        <div className="row">
                            <div className="col-lg-12 col-md-12 col-xs-12 col-sm-12 padding-20">
                                <div className=" padding-20">
                                    <div className="padding-20 border-blue">
                                        <h3>آیا از ادامه عملیات حذف اطمینان دارید؟</h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="modal-footer">
                        <span className="btn-waiting" ></span>
                        <input type="button" value="حذف" className="btn btn-block btn-default" onClick={this.deleteItem} />

                    </div>
                </div>
            </div>
        )
    }

}

ReactDOM.render(
    <FactorDetail />,
    document.getElementById('addFactoeDetailModal')
);

ReactDOM.render(
    <FactorForm />,
    document.getElementById('factorContext')
);


ReactDOM.render(
    <FactorItemDelete />,
    document.getElementById('removeFactoeDetailModal')
);
