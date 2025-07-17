import React from 'react';
import PropTypes from 'prop-types';
function AdditionalInfo() {
    return (
        <div className="row mt-4">
            <div className="col-12">
                <div className="card bg-light">
                    <div className="card-body">
                        <div className="row text-center">
                            <div className="col-md-3">
                                <div className="d-flex justify-content-center align-items-center mb-2">
                                    <i className="fas fa-shield-alt fa-2x text-success me-2"></i>
                                    <div>
                                        <strong>Safety</strong>
                                        <div className="small text-muted">
                                            Proven facilities
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-3">
                                <div className="d-flex justify-content-center align-items-center mb-2">
                                    <i className="fas fa-clock fa-2x text-primary me-2"></i>
                                    <div>
                                        <strong>24/7</strong>
                                        <div className="small text-muted">
                                            Customer Support
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-3">
                                <div className="d-flex justify-content-center align-items-center mb-2">
                                    <i className="fas fa-star fa-2x text-warning me-2"></i>
                                    <div>
                                        <strong>Качество</strong>
                                        <div className="small text-muted">
                                            Лучшие условия
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-3">
                                <div className="d-flex justify-content-center align-items-center mb-2">
                                    <i className="fas fa-handshake fa-2x text-info me-2"></i>
                                    <div>
                                        <strong>Гарантия</strong>
                                        <div className="small text-muted">
                                            Возврат средств
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default AdditionalInfo;
