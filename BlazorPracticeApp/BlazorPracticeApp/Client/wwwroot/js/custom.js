function show_message(message) {
    console.log('from custom js', message);
}

function dotnetInvocation() {
    DotNet.invokeMethodAsync("BlazorPracticeApp.Client", "GetCurrentCount").then(result => {
        console.log("count from js : ", result);
    });
}
function dotnetIncrementCount(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("IncrementCount");
}