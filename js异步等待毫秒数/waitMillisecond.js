/**
 * 等待毫秒数，示例： await waitMillisecond(2000)
 * @param {*} ms 等待的毫秒数值
 * @returns 返回一个 Promise 对象
 */
const waitMillisecond = (ms = 1000) => {
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve(1);
        }, ms);
    });
}

export default waitMillisecond;