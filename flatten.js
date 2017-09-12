function flatten (a, aout) {    
    if (aout === undefined)
        aout = [];

    for(var i = 0, len = a.length; i < len; i++) {
        if (Array.isArray(a[i])) {
            flatten(a[i], aout);
        } else {
            console.log("Found " + a[i]);
            aout.push(a[i]);
        }
    }

    return aout;
}

var a = [[5,[3], 4, [3,[2, [8]]]], 2];
console.log(flatten(a));