import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

// Interfaz para tipado estricto de datos (TypeScript)
export interface Shipment {
  id: number;
  tracking_number: string;
  status: string;
  estimated_delivery: string;
}

@Injectable({
  providedIn: 'root'
})
export class ShipmentService {
  private apiUrl = 'https://api.logistics-zinnia.com/v1/shipments'; // URL simulada

  constructor(private http: HttpClient) { }

  /**
   * Obtiene los embarques desde el backend consumiendo la API REST
   */
  getShipments(status?: string): Observable<Shipment[]> {
    let params = new HttpParams();
    if (status) {
      params = params.set('status', status);
    }

    // Uso de HttpClient y mapeo de respuestas estructuradas
    return this.http.get<any>(this.apiUrl, { params }).pipe(
      map(response => response.data.data as Shipment[]),
      catchError(error => {
        console.error('Error en la petición HTTP de logística:', error);
        throw error;
      })
    );
  }
}
