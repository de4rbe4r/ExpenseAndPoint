import { React, useState, useEffect } from 'react';
import axios from 'axios';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import { DeleteExpenseUrl, DeleteCategoryUrl} from "../Urls/UrlList";
import Button from 'react-bootstrap/Button';
import AlertModal from './AlertModal.jsx';
import Cookies from 'universal-cookie';



const ConfirmationForm = ({ expense, category, isShowForm, setIsShowForm, setIsDataUpdated, isDataUpdated }) => {
    const [title, setTitle] = useState();
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");
    const cookies = new Cookies();
    const config = {
        headers: { Authorization: `Bearer ${cookies.get('access_token')}` }
    };

    useEffect(() => {
        if (expense !== undefined) {
            setTitle("Удаление расхода");
        } else {
            setTitle("Удаление категории");
        }
    }, [isShowForm]);


    const handleCloseForm = () => {
        setIsShowForm(false);
    }

    const DeleteExpense = (event) => {
        event.preventDefault();
        console.log(config);
        if (expense !== undefined) {
            axios.delete(DeleteExpenseUrl + expense.id, config)
                .then(res => {
                    setShowAlertModal(true);
                    setErrorMessage("Расход успешно удален");
                    setTitleAlert("Успешно");
                })
                .catch(function (error) {
                    if (error.response) {
                        setShowAlertModal(true);
                        setErrorMessage(error.message + ". " + error.response.data.detail);
                        setTitleAlert("Ошибка");
                    }
                });
        } else if (category !== undefined) {
            axios.delete(DeleteCategoryUrl + category.id, config)
                .then(res => {
                    setShowAlertModal(true);
                    setErrorMessage("Категория успешно удалена");
                    setTitleAlert("Успешно");
                })
                .catch(function (error) {
                    if (error.response) {
                        setShowAlertModal(true);
                        setErrorMessage(error.message + ". " + error.response.data.detail);
                        setTitleAlert("Ошибка");
                    }
                });
        }
  
        setIsShowForm(false);
        setIsDataUpdated(!isDataUpdated);
    }

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }

    if (expense === undefined && category === undefined) return;


    return (
        <>
            <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
            <Modal
                show={isShowForm}
                onHide={handleCloseForm}
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>{title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {(
                        (category !== undefined && expense !== undefined) ? (
                            <>
                            <Form.Label htmlFor="category">Категория</Form.Label>
                                <Form.Control
                                    id="category"
                                type="text"
                                placeholder={category.title}
                                readOnly
                                />
                                <Form.Label htmlFor="date">Дата</Form.Label>
                                <Form.Control
                                    id="date"
                                    type="text"
                                    placeholder={expense.dateTime.substr(0, 10)}
                                    readOnly
                                />
                                <Form.Label htmlFor="time">Время</Form.Label>
                                <Form.Control
                                    id="time"
                                    type="text"
                                    placeholder={expense.dateTime.substr(11, 5)}
                                    readOnly
                                />
                                <Form.Label htmlFor="amount">Сумма</Form.Label>
                                <Form.Control
                                    id="amount"
                                    type="text"
                                    placeholder={expense.amount}
                                    readOnly
                                />
                            </>) : 
                            ((category !== undefined) ? (
                                <>
                            <Form.Label htmlFor="category">Категория</Form.Label>
                                <Form.Control
                                    id="category"
                                type="text"
                                placeholder={category.title}
                                readOnly
                                />
                                </>
                        ) : null)                      
                    ) }
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="danger" onClick={DeleteExpense}>Удалить</Button>
                </Modal.Footer>
            </Modal>
        </>
    )
}

export default ConfirmationForm;