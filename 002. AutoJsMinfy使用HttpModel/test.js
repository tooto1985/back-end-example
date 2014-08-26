//測試用的js檔案(故意寫錯)

var text = "test";
var hasData = true;
var newFunction = function (hasData) {

    hasData = "112";
    var if="if";
    012345
}


function Qoo(text) {
    var start = "[";
    var end = "]";

    start += "\"";
    end = "\"" + end;

    return  start + text + end;    

    
}

document.body.innerText = Qoo(text);