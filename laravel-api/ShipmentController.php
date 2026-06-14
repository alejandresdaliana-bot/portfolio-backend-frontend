<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Shipment;
use Illuminate\Http\Request;
use Illuminate\Http\JsonResponse;

class ShipmentController extends Controller
{
    /**
     * Obtener el listado de embarques filtrados por estado de forma optimizada.
     */
    public function index(Request $request): JsonResponse
    {
        try {
            // Demostración de filtrado lógico y Query Scopes de Eloquent
            $status = $request->query('status');

            $shipments = Shipment::select(['id', 'tracking_number', 'client_id', 'status', 'estimated_delivery', 'created_at'])
                ->when($status, function ($query, $status) {
                    return $query->where('status', $status);
                })
                ->with('client:id,name') // Carga relacional optimizada (Eager Loading)
                ->orderBy('created_at', 'desc')
                ->paginate(15);

            return response()->json([
                'success' => true,
                'message' => 'Listado de embarques recuperado con éxito.',
                'data' => $shipments
            ], 200);

        } catch (\Exception $e) {
            return response()->json([
                'success' => false,
                'message' => 'Error interno en el servidor logístico.',
                'error' => $e->getMessage()
            ], 500);
        }
    }
}
