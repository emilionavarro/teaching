function isDefined(x) {
    if (x !== undefined)
        return true;
    else
        return false;
}

function flatten (a, aout, index) {    
    if (aout === undefined)
        aout = [];
    if (index === undefined)
	index = 0;

    var level = "";
    var arrayString = JSON.stringify(a);

    for( var i = 0; i < index; i++){
	level += "--";
    }

    console.log(level + arrayString);

    for (var i = 0, len = a.length; i < len; i++) {
        if (Array.isArray(a[i])) {
            flatten(a[i], aout, index + 1);
        } else {
            aout.push(a[i]);
            console.log(level + "--" + a[i]);
        }
    }

    return aout;
}

function process () {
    var getAverage = function (a) {
        var average = 0;
        var length = void 0;

        if (a !== undefined) {
            len = a.length;

            for (var i = 0; i < len; i++) {
                average += a[i];    
            }

            average /= len;
        }

        return average;
    }

    var getMax = function (a) {
        var max = 0;

        if (isDefined(a)) {
            for (var i = 0, len = a.length; i < len; i++) {
                if (max < a[i])
                    max = a[i];
            }
        }

        return max;
    }

    var a = [[5,[3], 4, [3,[2, [8]]]], 2];
    var result = flatten(a);

    console.log(result);
    callCallback(getMax, result);
    callCallback(getAverage, result);
}

function callCallback(callback, parameter) {
    console.log("This callback returned: " + callback(parameter));
}


process();