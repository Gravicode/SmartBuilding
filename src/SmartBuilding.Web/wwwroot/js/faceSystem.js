        
async function face(labels){
        
        const MODEL_URL = '/models'

        await faceapi.loadSsdMobilenetv1Model(MODEL_URL)
        await faceapi.loadFaceLandmarkModel(MODEL_URL)
        await faceapi.loadFaceRecognitionModel(MODEL_URL)
        await faceapi.loadFaceExpressionModel(MODEL_URL)

        const img= document.getElementById('originalImg')
        let faceDescriptions = await faceapi.detectAllFaces(img).withFaceLandmarks().withFaceDescriptors().withFaceExpressions()
        const canvas = $('#reflay').get(0)
        faceapi.matchDimensions(canvas, img)

        faceDescriptions = faceapi.resizeResults(faceDescriptions, img)
        faceapi.draw.drawDetections(canvas, faceDescriptions)
        faceapi.draw.drawFaceLandmarks(canvas, faceDescriptions)
        faceapi.draw.drawFaceExpressions(canvas, faceDescriptions)

        
        //const labels = ['ross', 'rachel', 'chandler', 'monica', 'phoebe', 'joey']

        const labeledFaceDescriptors = await Promise.all(
            labels.map(async label => {

                //const imgUrl = `images/${label}.jpg`
                const imgUrl = label.url
                const img = await faceapi.fetchImage(imgUrl)
                
                const faceDescription = await faceapi.detectSingleFace(img).withFaceLandmarks().withFaceDescriptor()
                
                if (!faceDescription) {
                throw new Error(`no faces detected for ${label.nama}`)
                }
                
                const faceDescriptors = [faceDescription.descriptor]
                return new faceapi.LabeledFaceDescriptors(label.nama, faceDescriptors)
            })
        );

        const threshold = 0.6
        const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors, threshold)

        const results = faceDescriptions.map(fd => faceMatcher.findBestMatch(fd.descriptor))

        results.forEach((bestMatch, i) => {
            const box = faceDescriptions[i].detection.box
            const text = bestMatch.toString()
            const drawBox = new faceapi.draw.DrawBox(box, { label: text })
            drawBox.draw(canvas)
        })

}
var modelloaded = false;
async function detect_face(labels,camid,canvasid) {

    if (!modelloaded) {
        const MODEL_URL = '/models'

        await faceapi.loadSsdMobilenetv1Model(MODEL_URL)
        await faceapi.loadFaceLandmarkModel(MODEL_URL)
        await faceapi.loadFaceRecognitionModel(MODEL_URL)
        await faceapi.loadFaceExpressionModel(MODEL_URL)
        modelloaded = true;
    }
    const img = document.getElementById(camid)
    let faceDescriptions = await faceapi.detectAllFaces(img).withFaceLandmarks().withFaceDescriptors().withFaceExpressions()
    const canvas = $('#' + canvasid).get(0)
    faceapi.matchDimensions(canvas, img)

    faceDescriptions = faceapi.resizeResults(faceDescriptions, img)
    faceapi.draw.drawDetections(canvas, faceDescriptions)
    faceapi.draw.drawFaceLandmarks(canvas, faceDescriptions)
    faceapi.draw.drawFaceExpressions(canvas, faceDescriptions)


    //const labels = ['ross', 'rachel', 'chandler', 'monica', 'phoebe', 'joey']

    const labeledFaceDescriptors = await Promise.all(
        labels.map(async label => {

            //const imgUrl = `images/${label}.jpg`
            const imgUrl = label.url
            const img = await faceapi.fetchImage(imgUrl)

            const faceDescription = await faceapi.detectSingleFace(img).withFaceLandmarks().withFaceDescriptor()

            if (!faceDescription) {
                throw new Error(`no faces detected for ${label.nama}`)
            }

            const faceDescriptors = [faceDescription.descriptor]
            return new faceapi.LabeledFaceDescriptors(label.nama, faceDescriptors)
        })
    );

    const threshold = 0.6
    const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors, threshold)

    const results = faceDescriptions.map(fd => faceMatcher.findBestMatch(fd.descriptor))
    var persons = [];
    results.forEach((bestMatch, i) => {
        const box = faceDescriptions[i].detection.box
        const text = bestMatch.toString()
        const drawBox = new faceapi.draw.DrawBox(box, { label: text })
        drawBox.draw(canvas)
        const person = {};
        person.nama = text;
        person.url = img.src;
        person.box = faceDescriptions[i].detection.box;
        persons.push(person);
    })
    return persons;
}
    