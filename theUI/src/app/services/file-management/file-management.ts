import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';

interface File {
  code: string;
}

@Injectable({
  providedIn: 'root',
})
export class FileManagement {
  private http = inject(HttpClient);

  async getFileCode(): Promise<File> {
    const apiUrl: string = 'http://localhost:5299/getFileData';

    try {
      return await firstValueFrom(this.http.get<File>(apiUrl));
    } catch (error) {
      console.error(error);
      return { code: '' };
    }
  }

  async updateFile(code: string): Promise<any> {
    const apiUrl = 'http://localhost:5299/saveFile';

    try {
      return await firstValueFrom(
        this.http.post(apiUrl, { Base64Data: this.stringToBase64(code) }),
      );
    } catch (error) {
      console.error(error);
    }
  }

  stringToBase64(str: string): string {
    const binaryString = btoa(unescape(encodeURIComponent(str)));
    return binaryString;
  }
}
