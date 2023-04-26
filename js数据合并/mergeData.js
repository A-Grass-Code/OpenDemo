export default class MergeData {
    static test() {
        const a = {
            a: 1,
            b: {
                b1: 10,
                b2: [2, 21, 23]
            },
            c: [
                {
                    c1: 3,
                    c2: [44, 45],
                    c3: '',
                    c4: 0
                }
            ]
        }
        console.log('a --', a);

        const b = {
            a: 19,
            b: {
                b1: 190,
                b2: [291, 293]
            },
            c: [
                {
                    c4: 39,
                    c2: [495]
                },
                {
                    c4: 393,
                    c2: [4945]
                },
            ]
        }
        console.log('b --', b);

        console.log('源数据(b) 合并到 目标数据(a) 上 --', MergeData.setDataMerge(a, b));
    }

    /**
     * 设置数据合并，把 源数据 合并到 目标数据 上
     * @param {*} target 目标数据
     * @param {*} sources 源数据
     * @returns 返回新的目标数据
     */
    static setDataMerge(target, sources) {
        const targetDeep = JSON.parse(JSON.stringify(target));
        const sourcesDeep = JSON.parse(JSON.stringify(sources));
        let res = JSON.parse(JSON.stringify(target));
        if (targetDeep.constructor === Object && sourcesDeep.constructor === Object) {
            for (let key in targetDeep) {
                if (sourcesDeep.hasOwnProperty(key)) {
                    res[key] = MergeData.setDataMerge(targetDeep[key], sourcesDeep[key]);
                }
            }
        }
        else if (targetDeep.constructor === Array && sourcesDeep.constructor === Array) {
            if (targetDeep.length > 0) {
                const targetModel = targetDeep.slice(0, 1)[0];
                if (sourcesDeep.length > 0) {
                    const n = targetDeep.length >= sourcesDeep.length ? targetDeep.length : sourcesDeep.length;
                    for (let i = 0; i < n; i++) {
                        if (targetDeep.length >= sourcesDeep.length) {
                            if (i < sourcesDeep.length) {
                                res[i] = MergeData.setDataMerge(targetDeep[i], sourcesDeep[i]);
                            }
                        }
                        else {
                            if (i < targetDeep.length) {
                                res[i] = MergeData.setDataMerge(targetDeep[i], sourcesDeep[i]);
                            }
                            else {
                                res.push(MergeData.setDataMerge(targetModel, sourcesDeep[i]));
                            }
                        }
                    }
                }
            }
            else {
                res = [...sourcesDeep];
            }
        }
        else {
            res = sourcesDeep;
        }
        return res;
    }
}