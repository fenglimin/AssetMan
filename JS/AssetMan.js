function ShowDayDetail(linkCtrl) {
    if (linkCtrl.innerText == '显示资产详单') {
        linkCtrl.innerText = '隐藏资产详单';
        self.parent.document.getElementById("DivCenter").style["width"] = "850px";
        self.parent.document.getElementById("DivRight").style["width"] = "230px";
    } else {
        linkCtrl.innerText = '显示资产详单';
        self.parent.document.getElementById("DivCenter").style["width"] = "1080px";
        self.parent.document.getElementById("DivRight").style["width"] = "0px";
    }
}

function RefreshAssetDetail(reason) {
    self.parent.document.getElementById("urlFrameRight").src = 'Form/AssetDetailForm.aspx?Reason=' + reason;
}

function go(loc) {
    self.parent.document.getElementById("urlFrameCenter").src = loc;
}

function UpdateSelectEditCtrl(ucName) {
    if (document.getElementById(ucName + '_cbSave').checked) {
        var inputValue = document.getElementById(ucName + '_tbEdit').value;
        
        var ctrl = document.getElementById(ucName + '_lbSelect');
        for (var i = 0; i < ctrl.options.length; i++) {
            if (ctrl.options[i].value == inputValue)
                return;
        }

        var newOption = new Option(inputValue, inputValue);
        ctrl.options.add(newOption);
    }
}

function AddOil_OnOk(ucSelectEditCtrlName) {
    UpdateSelectEditCtrl(ucSelectEditCtrlName);
    RefreshAssetDetail('加油');
}

function Transfer_OnOk(desc) {
    RefreshAssetDetail(desc);
}

function OnSelectEditChanged(ucName, value) {
    document.getElementById(ucName + '_tbEdit').value = value;
    var checkSave = document.getElementById(ucName + '_cbSave');
    checkSave.checked = false;
    checkSave.disabled = true;

    var btDelete = document.getElementById(ucName + '_btDelete');
    btDelete.disable = false;
}

function OnInput(ucName, value) {
    var ctrl = document.getElementById(ucName + '_lbSelect');
    var checkSave = document.getElementById(ucName + '_cbSave');
    var btDelete = document.getElementById(ucName + '_btDelete');
    for (var i = 0; i < ctrl.options.length; i++) {
        if (ctrl.options[i].value == value) {
            checkSave.checked = false;
            checkSave.disabled = true;
            checkSave.parentNode.class = "disabled";
            btDelete.disable = false;
            btDelete.parentNode.class = "enabled";
            return;
        }
    }

    checkSave.disabled = false;
    checkSave.parentNode.class = "enabled";
    btDelete.disable = true;
    btDelete.parentNode.class = "disabled";
}

//function OnSelectEditChanged(ucName, value) {
//    document.getElementById('<%=tbEdit.ClientID %>').value = value;
//    document.getElementById('<%=cbSave.ClientID %>').checked = false;
//}

function MoneyInOut_OnOk(ucSelectEditCtrlName, desc) {
    UpdateSelectEditCtrl(ucSelectEditCtrlName);
    RefreshAssetDetail(desc);
}

function checkAll(cbAll, otherIds) {
    var iDs = otherIds.split(" ");
    for (var i = 1; i < iDs.length; i++) {
        document.getElementById(iDs[i]).checked = cbAll.checked;
    }
    
}

function checkMustSelect(cbCurrent, otherIds) {
    if (cbCurrent.checked)
        return;

    var iDs = otherIds.split(" ");
    for (var i = 1; i < iDs.length; i++) {
        if (document.getElementById(iDs[i]).checked)
            return;
    }

    alert('请至少选择一项！');
    cbCurrent.checked = true;
}

//function checkMe(cbAll) {
//    alert('aaaaa');
//}