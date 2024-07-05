import axios from 'axios';

class FileService {
    http = axios.create({
        baseURL: "https://localhost:7038/api/"
    })

    async fetchAllContainerAndBlobs() {
        const response = await this.http.get("File/AllContainerandBlobs");
        return response.data;
    }

    async uploadBlob(file: File) {
        const formData = new FormData();
        formData.append('file', file);

        const response = await this.http.post("File/AddBlobData", formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });
        return response.data;
    }

}

export default new FileService();
